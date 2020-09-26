using System.Runtime.InteropServices.ComTypes;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class CardDialog: DialogBase
    {
        private class Model
        {
            public int SelectedCardLevel { get; set; }
            public CardType SelectedCardType { get; set; }
            public int SelectedDeckId { get; set; }
        }

        private enum Mode
        {
            None = 0,
            CardViewer = 1,
            DeckBuilder = 2
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string ViewCardLevelsPageId = "VIEW_CARD_LEVELS_PAGE";
        private const string ViewCardsPageId = "VIEW_CARDS_PAGE";
        private const string ViewCardDetailsPageId = "VIEW_CARD_DETAILS_PAGE";
        private const string DeckListPageId = "DECK_LIST_PAGE";
        private const string DeckDetailsPageId = "DECK_DETAILS_PAGE";

        // Gui details
        private const int CardParts = 6; // Top, Left/Right, Bottom, Background, Graphic, Element (Note: Left/Right are on the same Id)
        private const int DeckSize = 5;
        private static Gui.IdReservation _idReservation;
        private const string TextureGlyph = "a";

        [NWNEventHandler("mod_load")]
        public static void ReserveGuiIds()
        {
            _idReservation = Gui.ReserveIds(nameof(CardDialog), CardParts * DeckSize);
        }

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddBackAction(Back)
                .AddEndAction(ClearTemporaryVariables)
                .AddPage(MainPageId, MainPageInit)
                .AddPage(ViewCardLevelsPageId, ViewCardLevelsPageInit)
                .AddPage(ViewCardsPageId, ViewCardsPageInit)
                .AddPage(ViewCardDetailsPageId, ViewCardDetailsPageInit)
                .AddPage(DeckListPageId, DeckListPageInit)
                .AddPage(DeckDetailsPageId, DeckDetailsPageInit);

            return builder.Build();
        }

        private void ClearTemporaryVariables()
        {
            var player = GetPC();
            DeleteLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
            DeleteLocalInt(player, "CARD_MENU_CURRENT_MODE");
        }

        private void Back(string oldPage, string newPage)
        {
            if (oldPage == ViewCardDetailsPageId)
            {
                DeleteLocalInt(GetPC(), "CARD_MENU_SELECTED_CARD_ID");
            }

            if (oldPage == DeckDetailsPageId)
            {
                DeleteLocalInt(GetPC(), "CARD_MENU_CURRENT_MODE");
            }
        }

        private void MainPageInit(DialogPage page)
        {
            var player = GetPC();
            page.Header = ColorToken.Green("Triple Triad Menu");

            page.AddResponse("View Cards", () =>
            {
                SetLocalInt(player, "CARD_MENU_CURRENT_MODE", (int)Mode.CardViewer);
                ChangePage(ViewCardLevelsPageId);
            });
            page.AddResponse("Manage Decks", () =>
            {
                ChangePage(DeckListPageId);
            });
        }

        private void ViewCardLevelsPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            page.Header = "Please select a card level.";

            for (var level = 1; level <= 10; level++)
            {
                var levelSelection = level; // Copy the value so the following delegate uses the correct number.
                page.AddResponse($"Level {level}", () =>
                {
                    model.SelectedCardLevel = levelSelection;
                    ChangePage(ViewCardsPageId);
                });
            }
        }

        private void ViewCardsPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var availableCards = TripleTriad.GetAllCardsAtLevel(model.SelectedCardLevel);

            page.Header = $"{ColorToken.Green("Level: ")} {model.SelectedCardLevel}\n\n" +
                $"The following is the list of cards available at this level. Cards in {ColorToken.Green("GREEN")} have been acquired. Those in {ColorToken.Red("RED")} have not. Only one card per type can be collected.\n\n" +
                "Please select a card.";

            foreach (var (type, card) in availableCards)
            {
                if (!card.IsVisibleInMenu) continue;

                var option = dbPlayerTripleTriad.AvailableCards.ContainsKey(type) 
                    ? ColorToken.Green(card.Name) 
                    : ColorToken.Red(card.Name);

                page.AddResponse(option, () =>
                {
                    model.SelectedCardType = type;
                    SetLocalInt(player, "CARD_MENU_SELECTED_CARD_ID", (int)model.SelectedCardType);
                    ChangePage(ViewCardDetailsPageId);
                });
            }

        }

        private void ViewCardDetailsPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var card = TripleTriad.GetCardByType(model.SelectedCardType);
            var dateAcquired = dbPlayerTripleTriad.AvailableCards.ContainsKey(model.SelectedCardType)
                ? dbPlayerTripleTriad.AvailableCards[model.SelectedCardType].ToString("yyyy-MM-dd hh:mm:ss")
                : ColorToken.Red("Unacquired");

            page.Header = $"{ColorToken.Green("Name: ")} {card.Name}\n" +
                          $"{ColorToken.Green("Level: ")} {card.Level}\n" +
                          $"{ColorToken.Green("Date Acquired: ")} {dateAcquired}";
        }

        private void DeckListPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();

            page.Header = "You may have a maximum of 20 decks saved at a time. Decks must contain exactly five cards. \n\nPlease select a deck.";

            for (var deck = 1; deck <= 20; deck++)
            {
                var deckName = "[None]";
                if (dbPlayerTripleTriad.Decks.ContainsKey(deck))
                {
                    deckName = dbPlayerTripleTriad.Decks[deck].Name;
                }

                var deckId = deck; // Copy the deck Id for use inside the delegate.
                page.AddResponse($"Deck #{deck}: {deckName}", () =>
                {
                    model.SelectedDeckId = deckId;
                    LoadDeckOntoPC();
                    SetLocalInt(player, "CARD_MENU_CURRENT_MODE", (int)Mode.DeckBuilder);
                    ChangePage(DeckDetailsPageId);
                });
            }
        }

        private void LoadDeckOntoPC()
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var deckId = model.SelectedDeckId;
            var cardId1 = (int)CardType.Invalid;
            var cardId2 = (int)CardType.Invalid;
            var cardId3 = (int)CardType.Invalid;
            var cardId4 = (int)CardType.Invalid;
            var cardId5 = (int)CardType.Invalid;

            if (dbPlayerTripleTriad.Decks.ContainsKey(deckId))
            {
                cardId1 = (int)dbPlayerTripleTriad.Decks[deckId].Card1;
                cardId2 = (int)dbPlayerTripleTriad.Decks[deckId].Card2;
                cardId3 = (int)dbPlayerTripleTriad.Decks[deckId].Card3;
                cardId4 = (int)dbPlayerTripleTriad.Decks[deckId].Card4;
                cardId5 = (int)dbPlayerTripleTriad.Decks[deckId].Card5;
            }

            SetLocalInt(player, "CARD_MENU_DECK_CARD_1", cardId1);
            SetLocalInt(player, "CARD_MENU_DECK_CARD_2", cardId2);
            SetLocalInt(player, "CARD_MENU_DECK_CARD_3", cardId3);
            SetLocalInt(player, "CARD_MENU_DECK_CARD_4", cardId4);
            SetLocalInt(player, "CARD_MENU_DECK_CARD_5", cardId5);

        }

        private void DeckDetailsPageInit(DialogPage page)
        {

        }

        [NWNEventHandler("interval_pc_1s")]
        public static void DisplayCards()
        {
            var player = OBJECT_SELF;
            var mode = (Mode)GetLocalInt(player, "CARD_MENU_CURRENT_MODE");

            // Card Viewer - Place the card inside the conversation box
            if (mode == Mode.CardViewer)
            {
                DisplayCardViewer(player);
            }
            // Deck Builder - Display all currently selected cards
            else if(mode == Mode.DeckBuilder)
            {
                DisplayDeck(player);
            }
        }

        private static void DisplayCardViewer(uint player)
        {
            var selectedCardId = GetLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
            if (selectedCardId <= 0) return;

            var selectedCardType = (CardType)selectedCardId;
            DrawCard(player, selectedCardType, 20, 7, 1);
        }

        private static void DisplayDeck(uint player)
        {
            var cardType1 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_1");
            var cardType2 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_2");
            var cardType3 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_3");
            var cardType4 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_4");
            var cardType5 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_5");

            DrawCard(player, cardType1, 37, 1, 1);
            DrawCard(player, cardType2, 53, 1, 2);
            DrawCard(player, cardType3, 69, 1, 3);
            DrawCard(player, cardType4, 37, 12, 4);
            DrawCard(player, cardType5, 53, 12, 5);
        }

        private static void DrawCard(uint player, CardType cardType, int x, int y, int deckSlot)
        {
            var card = TripleTriad.GetCardByType(cardType);
            var elementTexture = TripleTriad.GetElementTexture(card.Element);

            var startId = _idReservation.StartId + (deckSlot * CardParts);
            var topPower = card.TopPower > 9 ? "A" : card.TopPower.ToString();
            var bottomPower = card.BottomPower > 9 ? "A" : card.BottomPower.ToString();
            var leftPower = card.LeftPower > 9 ? "A" : card.LeftPower.ToString();
            var rightPower = card.RightPower > 9 ? "A" : card.RightPower.ToString();

            PostString(player, topPower, x + 3, y+1, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId, Gui.TextName);
            PostString(player, leftPower + " " + rightPower, x + 2, y+2, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId + 1, Gui.TextName);
            PostString(player, bottomPower, x + 3, y+3, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId + 2, Gui.TextName);
            PostString(player, TextureGlyph, x + 10, y + 1, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId + 3, elementTexture);
            PostString(player, TextureGlyph, x, y, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId + 4, card.Texture);
            PostString(player, TextureGlyph, x, y, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId + 5, "fnt_card_back");
        }

    }
}
