using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class TripleTriadPlayerDialog: DialogBase
    {
        private class Model
        {
            public int SelectedCardLevel { get; set; }
            public CardType SelectedCardType { get; set; }
            public int SelectedDeckId { get; set; }
            public bool IsConfirming { get; set; }
            public int CurrentCardNumber { get; set; }
            public Stack<DialogNavigation> NavigationStackBeforeDeckBuilding { get; set; }
        }

        private enum Mode
        {
            None = 0,
            CardViewer = 1,
            DeckBuilder = 2
        }

        // Page names
        private const string MainPageId = "MAIN_PAGE";
        private const string ViewCardLevelsPageId = "VIEW_CARD_LEVELS_PAGE";
        private const string ViewCardsPageId = "VIEW_CARDS_PAGE";
        private const string ViewCardDetailsPageId = "VIEW_CARD_DETAILS_PAGE";
        private const string DeckListPageId = "DECK_LIST_PAGE";
        private const string DeckDetailsPageId = "DECK_DETAILS_PAGE";
        private const string ChangeDeckNamePageId = "CHANGE_DECK_NAME_PAGE";
        private const string DeckCardSelectionLevelsPageId = "DECK_CARD_SELECTION_LEVELS_PAGE";
        private const string DeckCardSelectionListPageId = "DECK_CARD_SELECTION_LIST_PAGE";
        private const string DeckCardSelectionConfirmDeckPageId = "DECK_CARD_SELECTION_CONFIRM_DECK_PAGE";

        // Gui details
        private const int CardParts = 6; // Top, Left/Right, Bottom, Background, Graphic, Element (Note: Left/Right are on the same Id)
        private const int DeckSize = 5;
        private static Gui.IdReservation _idReservation;
        private const string TextureGlyph = "a";

        // Constants
        private const int MaxDeckNameLength = 30;

        /// <summary>
        /// When the module loads, reserve enough Gui Ids to display all necessary Gui elements.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void ReserveGuiIds()
        {
            _idReservation = Gui.ReserveIds(nameof(TripleTriadPlayerDialog), CardParts * DeckSize);
        }

        /// <summary>
        /// Sets up all pages.
        /// </summary>
        /// <param name="player">The player this dialog is being set up for.</param>
        /// <returns>The constructed player dialog</returns>
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
                .AddPage(DeckDetailsPageId, DeckDetailsPageInit)
                .AddPage(ChangeDeckNamePageId, ChangeDeckNamePageInit)
                .AddPage(DeckCardSelectionLevelsPageId, DeckCardSelectionLevelsPageInit)
                .AddPage(DeckCardSelectionListPageId, DeckCardSelectionListPageInit)
                .AddPage(DeckCardSelectionConfirmDeckPageId, DeckCardSelectionConfirmDeckPageInit);

            return builder.Build();
        }

        /// <summary>
        /// Clears all temporary variables stored on the player.
        /// </summary>
        private void ClearTemporaryVariables()
        {
            var player = GetPC();
            DeleteLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
            DeleteLocalInt(player, "CARD_MENU_CURRENT_MODE");
            DeleteLocalBool(player, "CARD_MENU_LISTENING_FOR_NAME_CHANGE");
            DeleteLocalString(player, "CARD_MENU_DECK_NAME");
        }

        /// <summary>
        /// Clears specific temporary variables stored on the player when they click the back button.
        /// </summary>
        /// <param name="oldPage">The old page</param>
        /// <param name="newPage">The new page</param>
        private void Back(string oldPage, string newPage)
        {
            var player = GetPC();
            var model = GetDataModel<Model>();
            model.IsConfirming = false;

            if (oldPage == ViewCardDetailsPageId)
            {
                DeleteLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
            }

            if (oldPage == DeckDetailsPageId)
            {
                DeleteLocalInt(player, "CARD_MENU_CURRENT_MODE");
            }

            if (oldPage == ChangeDeckNamePageId)
            {
                DeleteLocalBool(player, "CARD_MENU_LISTENING_FOR_NAME_CHANGE");
                DeleteLocalString(player, "CARD_MENU_DECK_NAME");
            }

            if (oldPage == DeckCardSelectionLevelsPageId)
            {
                LoadDeckOntoPC(model.SelectedDeckId);
            }

            if (oldPage == DeckCardSelectionConfirmDeckPageId)
            {
                RemoveTopMostCard();
            }
        }

        /// <summary>
        /// Handles main page logic. 
        /// </summary>
        /// <param name="page">The page to build</param>
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
                if (TripleTriad.DoesPlayerHavePendingGame(player))
                {
                    FloatingTextStringOnCreature("Decks cannot be managed while you have a game reservation pending. Cancel the reservation to manage your decks.", player, false);
                    return;
                }

                ChangePage(DeckListPageId);
            });
        }

        /// <summary>
        /// Handles the card level list logic.
        /// </summary>
        /// <param name="page">The page to build</param>
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

        /// <summary>
        /// Handles the card list logic.
        /// </summary>
        /// <param name="page">The page to build</param>
        private void ViewCardsPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();
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

        /// <summary>
        /// Handles the card details logic.
        /// </summary>
        /// <param name="page">The page to build</param>
        private void ViewCardDetailsPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();
            var model = GetDataModel<Model>();
            var card = TripleTriad.GetCardByType(model.SelectedCardType);
            var dateAcquired = dbPlayerTripleTriad.AvailableCards.ContainsKey(model.SelectedCardType)
                ? dbPlayerTripleTriad.AvailableCards[model.SelectedCardType].ToString("yyyy-MM-dd hh:mm:ss")
                : ColorToken.Red("Unacquired");

            page.Header = $"{ColorToken.Green("Name: ")} {card.Name}\n" +
                          $"{ColorToken.Green("Level: ")} {card.Level}\n" +
                          $"{ColorToken.Green("Date Acquired: ")} {dateAcquired}";
        }

        /// <summary>
        /// Handles the deck list logic.
        /// </summary>
        /// <param name="page">The page to build</param>
        private void DeckListPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();
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
                    LoadDeckOntoPC(deckId);
                    SetLocalInt(player, "CARD_MENU_CURRENT_MODE", (int)Mode.DeckBuilder);
                    ChangePage(DeckDetailsPageId);
                });
            }
        }

        /// <summary>
        /// Loads a player's deck onto their local variables.
        /// These local variables are picked up by the Gui display methods.
        /// This is necessary because script calls do not have access to the dialog's model data.
        /// </summary>
        /// <param name="deckId">The deck Id to load.</param>
        private void LoadDeckOntoPC(int deckId)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
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

        /// <summary>
        /// Handles the deck details logic.
        /// </summary>
        /// <param name="page">The page to build</param>
        private void DeckDetailsPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var deck = dbPlayerTripleTriad.Decks.ContainsKey(model.SelectedDeckId)
                ? dbPlayerTripleTriad.Decks[model.SelectedDeckId]
                : new CardDeck{ Name = $"Deck #{model.SelectedDeckId}"};

            page.Header = $"{ColorToken.Green("Deck Name: ")} {deck.Name}\n\n" +
                "What would you like to do with this deck?";

            // Deck Renaming
            page.AddResponse("Change Name", () =>
            {
                model.IsConfirming = false;
                ChangePage(ChangeDeckNamePageId);
            });

            // Card Selection
            page.AddResponse("Select Cards", () =>
            {
                model.IsConfirming = false;
                LoadDeckOntoPC(-1); // Intentionally blank out the card display since we're picking new cards.
                model.CurrentCardNumber = 1;

                // Clone the stack. We will revert back to this state when the player finishes building the deck.
                model.NavigationStackBeforeDeckBuilding = new Stack<DialogNavigation>(NavigationStack.Reverse());
                // The topmost navigation points back to the deck list page which leads to users having to click "Back" twice after confirming their deck selections.
                // Pop this since we only need one.
                model.NavigationStackBeforeDeckBuilding.Pop(); 

                ChangePage(DeckCardSelectionLevelsPageId);
            });

            // Deck Deletion
            if (model.IsConfirming)
            {
                page.AddResponse($"{ColorToken.Red("CONFIRM DELETE DECK")}", () =>
                {
                    if (dbPlayerTripleTriad.Decks.ContainsKey(model.SelectedDeckId))
                    {
                        dbPlayerTripleTriad.Decks.Remove(model.SelectedDeckId);
                        DB.Set(playerId, dbPlayerTripleTriad);
                    }

                    LoadDeckOntoPC(model.SelectedDeckId);
                    model.IsConfirming = false;
                });
            }
            else
            {
                page.AddResponse($"{ColorToken.Red("Delete Deck")}", () =>
                {
                    model.IsConfirming = true;
                });
            }
        }

        /// <summary>
        /// Handles the deck name change logic.
        /// </summary>
        /// <param name="page">The page to build</param>
        private void ChangeDeckNamePageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var deck = dbPlayerTripleTriad.Decks.ContainsKey(model.SelectedDeckId)
                ? dbPlayerTripleTriad.Decks[model.SelectedDeckId]
                : new CardDeck();
            var deckName = string.IsNullOrWhiteSpace(deck.Name) ? "[None]" : deck.Name;
            var selectedDeckName = GetLocalString(player, "CARD_MENU_DECK_NAME");
            var newDeckName = selectedDeckName;
            if (string.IsNullOrWhiteSpace(newDeckName))
                newDeckName = ColorToken.Yellow("[NO CHANGE]");

            page.Header = $"{ColorToken.Green("Current Deck Name: ")} {deckName}\n" +
                $"{ColorToken.Green("New Deck Name: ")} {newDeckName}\n\n" +
                $"Please type the new name of your deck into the chat bar. Note that deck names may be no longer than {MaxDeckNameLength} characters. Use 'Refresh' to see the changes. Use 'Set Name' to confirm the change.";

            page.AddResponse(ColorToken.Green("Refresh"), () => { });

            if (!string.IsNullOrWhiteSpace(selectedDeckName))
            {
                page.AddResponse("Set Name", () =>
                {
                    deck.Name = selectedDeckName;
                    DeleteLocalString(player, "CARD_MENU_DECK_NAME");

                    DB.Set(playerId, dbPlayerTripleTriad);
                });
            }

            SetLocalBool(player, "CARD_MENU_LISTENING_FOR_NAME_CHANGE", true);
        }

        /// <summary>
        /// Handles the card selection level logic for deck building.
        /// This differs from the card viewer in that levels show based on which cards the player owns.
        /// </summary>
        /// <param name="page">The page to build.</param>
        private void DeckCardSelectionLevelsPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var levels = new HashSet<int>();

            page.Header = "Please select cards to add to this deck.";

            if (model.CurrentCardNumber > 1)
            {
                page.AddResponse(ColorToken.Red("Remove Card"), RemoveTopMostCard);
            }

            foreach (var (cardType, _) in dbPlayerTripleTriad.AvailableCards)
            {
                var card = TripleTriad.GetCardByType(cardType);

                if (!card.IsVisibleInMenu) continue;

                if (!levels.Contains(card.Level))
                    levels.Add(card.Level);
            }

            page.Header = $"{ColorToken.Green("Select card #")} {model.CurrentCardNumber}";

            foreach (var level in levels)
            {
                page.AddResponse($"Level {level}", () =>
                {
                    model.SelectedCardLevel = level;
                    ChangePage(DeckCardSelectionListPageId);
                });
            }
        }

        /// <summary>
        /// Handles the card selection logic for deck building.
        /// This differs from the card viewer in that cards show based on which cards the player owns.
        /// </summary>
        /// <param name="page">The page to build.</param>
        private void DeckCardSelectionListPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var cardsAtLevel = TripleTriad.GetAllCardsAtLevel(model.SelectedCardLevel);

            page.Header = $"{ColorToken.Green("Level: ")} {model.SelectedCardLevel}\n\n" +
                "Please select cards to add to this deck.";

            if (model.CurrentCardNumber > 1)
            {
                page.AddResponse(ColorToken.Red("Remove Card"), RemoveTopMostCard);
            }

            foreach (var (cardType, card) in cardsAtLevel)
            {
                if (!card.IsVisibleInMenu) continue;

                // Only one of each card type can be added to decks.
                if(IsInActiveDeck(cardType)) continue;

                if (dbPlayerTripleTriad.AvailableCards.ContainsKey(cardType))
                {
                    page.AddResponse($"Add: {card.Name}", () =>
                    {
                        SetLocalInt(player, $"CARD_MENU_DECK_CARD_{model.CurrentCardNumber}", (int)cardType);
                        model.CurrentCardNumber++;

                        // This was the last card. 
                        if (model.CurrentCardNumber > 5)
                        {
                            ChangePage(DeckCardSelectionConfirmDeckPageId);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Handles the card selection 'confirm deck' logic.
        /// </summary>
        /// <param name="page">The page to build.</param>
        private void DeckCardSelectionConfirmDeckPageInit(DialogPage page)
        {
            page.Header = "Is this deck okay?";

            page.AddResponse($"{ColorToken.Green("Confirm Deck")}", () =>
            {
                var player = GetPC();
                var playerId = GetObjectUUID(player);
                var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
                var model = GetDataModel<Model>();
                var deck = dbPlayerTripleTriad.Decks.ContainsKey(model.SelectedDeckId)
                    ? dbPlayerTripleTriad.Decks[model.SelectedDeckId]
                    : new CardDeck {Name = "New Deck"};

                deck.Card1 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_1");
                deck.Card2 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_2");
                deck.Card3 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_3");
                deck.Card4 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_4");
                deck.Card5 = (CardType)GetLocalInt(player, "CARD_MENU_DECK_CARD_5");

                dbPlayerTripleTriad.Decks[model.SelectedDeckId] = deck;
                DB.Set(playerId, dbPlayerTripleTriad);

                DeleteLocalInt(player, "CARD_MENU_CURRENT_MODE");

                NavigationStack = model.NavigationStackBeforeDeckBuilding;
                ChangePage(DeckListPageId, false);
            });
        }

        /// <summary>
        /// Removes the top-most card from the deck builder.
        /// </summary>
        private void RemoveTopMostCard()
        {
            var player = GetPC();
            var model = GetDataModel<Model>();
            var cardNumber = model.CurrentCardNumber;
            cardNumber--;

            DeleteLocalInt(player, $"CARD_MENU_DECK_CARD_{cardNumber}");

            if (cardNumber < 1)
                cardNumber = 1;

            model.CurrentCardNumber = cardNumber;
        }

        /// <summary>
        /// Checks whether the specified card is in the deck the player is actively building.
        /// </summary>
        /// <param name="cardType">The card type to look for.</param>
        /// <returns>true if the card exists in the active deck, false otherwise</returns>
        private bool IsInActiveDeck(CardType cardType)
        {
            var player = GetPC();
            var cardTypeId = (int) cardType;

            if (GetLocalInt(player, "CARD_MENU_DECK_CARD_1") == cardTypeId ||
                GetLocalInt(player, "CARD_MENU_DECK_CARD_2") == cardTypeId ||
                GetLocalInt(player, "CARD_MENU_DECK_CARD_3") == cardTypeId ||
                GetLocalInt(player, "CARD_MENU_DECK_CARD_4") == cardTypeId ||
                GetLocalInt(player, "CARD_MENU_DECK_CARD_5") == cardTypeId)
                return true;

            return false;
        }

        /// <summary>
        /// Handles listening for deck name changes.
        /// </summary>
        [NWNEventHandler("on_nwnx_chat")]
        public static void ListenForDeckNameChange()
        {
            var player = Chat.GetSender();
            if (!GetLocalBool(player, "CARD_MENU_LISTENING_FOR_NAME_CHANGE")) return;

            var message = Chat.GetMessage();
            if (message.Length > MaxDeckNameLength)
            {
                message = message.Substring(0, MaxDeckNameLength);
            }

            SetLocalString(player, "CARD_MENU_DECK_NAME", message);
            SendMessageToPC(player, "Press 'Refresh' to see changes.");

            Chat.SkipMessage();
        }

        /// <summary>
        /// Handles displaying cards on-screen with PostString.
        /// </summary>
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

        /// <summary>
        /// Handles displaying the card viewer textures.
        /// </summary>
        /// <param name="player">The player to draw for.</param>
        private static void DisplayCardViewer(uint player)
        {
            var selectedCardId = GetLocalInt(player, "CARD_MENU_SELECTED_CARD_ID");
            if (selectedCardId <= 0) return;

            var selectedCardType = (CardType)selectedCardId;
            DrawCard(player, selectedCardType, 20, 7, 1);
        }

        /// <summary>
        /// Handles displaying a deck's card textures.
        /// </summary>
        /// <param name="player">The player to draw for</param>
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

        /// <summary>
        /// Draws a single card at a location on the player's screen.
        /// </summary>
        /// <param name="player">The player to draw for.</param>
        /// <param name="cardType">The type of card to draw on screen.</param>
        /// <param name="x">The X coordinate to draw at.</param>
        /// <param name="y">The Y coordinate to draw at.</param>
        /// <param name="deckSlot">The deck card slot to draw at (between 1 and 5)</param>
        private static void DrawCard(uint player, CardType cardType, int x, int y, int deckSlot)
        {
            var card = TripleTriad.GetCardByType(cardType);
            var elementTexture = TripleTriad.GetElementTexture(card.Element);

            var startId = _idReservation.StartId + (deckSlot * CardParts);
            var topPower = card.TopPower > 9 ? "A" : card.TopPower.ToString();
            var bottomPower = card.BottomPower > 9 ? "A" : card.BottomPower.ToString();
            var leftPower = card.LeftPower > 9 ? "A" : card.LeftPower.ToString();
            var rightPower = card.RightPower > 9 ? "A" : card.RightPower.ToString();

            // Display the power ranks if all of them are greater than zero.
            if (card.TopPower > 0 && card.BottomPower > 0 && card.LeftPower > 0 && card.RightPower > 0)
            {
                PostString(player, topPower, x + 3, y + 1, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId, Gui.TextName);
                PostString(player, leftPower + " " + rightPower, x + 2, y + 2, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId + 1, Gui.TextName);
                PostString(player, bottomPower, x + 3, y + 3, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId + 2, Gui.TextName);
            }
            // Otherwise assume a card isn't selected. Only display the card deck slot in this case.
            else
            {
                PostString(player, $"Card #{deckSlot}", x + 3, y + 1, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId, Gui.TextName);
            }

            PostString(player, TextureGlyph, x + 10, y + 1, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId + 3, elementTexture);
            PostString(player, TextureGlyph, x, y, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId + 4, card.Texture);
            PostString(player, TextureGlyph, x, y, ScreenAnchor.TopLeft, 1.1f, Gui.ColorWhite, Gui.ColorWhite, startId + 5, "fnt_card_back");
        }

    }
}
