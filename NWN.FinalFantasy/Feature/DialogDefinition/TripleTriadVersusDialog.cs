using System;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class TripleTriadVersusDialog: DialogBase
    {
        private class Model
        {
            public CardDeck NPCDeck { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddInitializationAction(Initialization)
                .AddPage(MainPageId, MainPageInit);

            return builder.Build();
        }

        private void Initialization()
        {
            var npc = OBJECT_SELF;
            var model = GetDataModel<Model>();

            model.NPCDeck = new CardDeck
            {
                Name = "NPC Deck",
                Card1 = (CardType) GetLocalInt(npc, "NPC_DECK_CARD_1"),
                Card2 = (CardType) GetLocalInt(npc, "NPC_DECK_CARD_2"),
                Card3 = (CardType) GetLocalInt(npc, "NPC_DECK_CARD_3"),
                Card4 = (CardType) GetLocalInt(npc, "NPC_DECK_CARD_4"),
                Card5 = (CardType) GetLocalInt(npc, "NPC_DECK_CARD_5"),
            };

            DeleteLocalInt(npc, "NPC_DECK_CARD_1");
            DeleteLocalInt(npc, "NPC_DECK_CARD_2");
            DeleteLocalInt(npc, "NPC_DECK_CARD_3");
            DeleteLocalInt(npc, "NPC_DECK_CARD_4");
            DeleteLocalInt(npc, "NPC_DECK_CARD_5");
        }

        private void MainPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();
            var model = GetDataModel<Model>();
            page.Header = "Which deck would you like to use?";

            page.AddResponse(ColorToken.Green("View Cards"), () => SwitchConversation(nameof(TripleTriadPlayerDialog)));

            foreach (var (_, deck) in dbPlayerTripleTriad.Decks)
            {
                if (deck.Card1 != CardType.Invalid &&
                    deck.Card2 != CardType.Invalid &&
                    deck.Card3 != CardType.Invalid &&
                    deck.Card4 != CardType.Invalid &&
                    deck.Card5 != CardType.Invalid)
                {
                    page.AddResponse($"Choose Deck: {deck.Name}", () =>
                    {
                        TripleTriad.StartGame(player, deck, OBJECT_SELF, model.NPCDeck);
                        EndConversation();
                    });
                }
            }
        }

    }
}
