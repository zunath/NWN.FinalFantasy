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
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string ViewCardLevelsPageId = "VIEW_CARD_LEVELS_PAGE";
        private const string ViewCardsPageId = "VIEW_CARDS_PAGE";
        private const string ViewCardDetailsPageId = "VIEW_CARD_DETAILS_PAGE";
        private const string ManageDecksPageId = "MANAGE_DECKS_PAGE";

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
                .AddBackAction((oldPage, newPage) => DeleteLocalInt(GetPC(), "CARD_MENU_SELECTED_CARD_ID"))
                .AddEndAction(ClearTemporaryVariables)
                .AddPage(MainPageId, MainPageInit)
                .AddPage(ViewCardLevelsPageId, ViewCardLevelsPageInit)
                .AddPage(ViewCardsPageId, ViewCardsPageInit)
                .AddPage(ViewCardDetailsPageId, ViewCardDetailsPageInit)
                .AddPage(ManageDecksPageId, ManageDecksPageInit);

            return builder.Build();
        }

        private void ClearTemporaryVariables()
        {
            var player = GetPC();
            DeleteLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
        }

        private void MainPageInit(DialogPage page)
        {
            page.Header = ColorToken.Green("Triple Triad Menu");

            page.AddResponse("View Cards", () => ChangePage(ViewCardLevelsPageId));
            page.AddResponse("Manage Decks", () => ChangePage(ManageDecksPageId));
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

        private void ManageDecksPageInit(DialogPage page)
        {

        }

        [NWNEventHandler("interval_pc_1s")]
        public static void DisplayCards()
        {
            var player = OBJECT_SELF;
            var selectedCardId = GetLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
            if (selectedCardId <= 0) return;

            var selectedCardType = (CardType) selectedCardId;
            DrawCard(player, selectedCardType, 37, 1, 1);
        }

        private static void DrawCard(uint player, CardType cardType, int x, int y, int deckSlot)
        {
            var card = TripleTriad.GetCardByType(cardType);

            var startId = _idReservation.StartId + (deckSlot * CardParts);
            PostString(player, TextureGlyph, x, y, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId, card.Texture);
            PostString(player, TextureGlyph, x, y, ScreenAnchor.TopLeft, 1f, Gui.ColorWhite, Gui.ColorWhite, startId + 1, "fnt_card_back");
        }

    }
}
