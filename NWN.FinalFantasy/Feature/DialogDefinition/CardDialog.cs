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

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddInitializationAction(() => SetLocalBool(player, "CARD_MENU_OPEN", true))
                .AddEndAction(() => DeleteLocalBool(player, "CARD_MENU_OPEN"))
                .AddPage(MainPageId, MainPageInit)
                .AddPage(ViewCardLevelsPageId, ViewCardLevelsPageInit)
                .AddPage(ViewCardsPageId, ViewCardsPageInit)
                .AddPage(ViewCardDetailsPageId, ViewCardDetailsPageInit)
                .AddPage(ManageDecksPageId, ManageDecksPageInit);

            return builder.Build();
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
                $"The following is the full list of cards available. Cards in {ColorToken.Green("GREEN")} have been acquired. Those in {ColorToken.Red("RED")} have not. Only one card per type can be collected.\n\n" +
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

            // todo: Display a PostString version of the card, power, and element
        }

        private void ManageDecksPageInit(DialogPage page)
        {

        }

        [NWNEventHandler("interval_pc_1s")]
        public static void DisplayCard()
        {
            var player = OBJECT_SELF;
            if (!GetLocalBool(player, "CARD_MENU_OPEN")) return;

            PostString(player, "a", 0, 0, ScreenAnchor.TopLeft, 10f, Gui.ColorWhite, Gui.ColorWhite, 8888, "fnt_card_1");
        }

    }
}
