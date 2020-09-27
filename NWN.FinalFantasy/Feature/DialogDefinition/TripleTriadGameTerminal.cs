using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class TripleTriadGameTerminal: DialogBase
    {
        private class Model
        {
            public int SelectedDeckId { get; set; }
            public bool IsConfirming { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string SelectDeckPageId = "SELECT_DECK_PAGE";


        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddInitializationAction(Initialize)
                .AddPage(MainPageId, MainPageInit)
                .AddPage(SelectDeckPageId, SelectDeckPageInit);

            return builder.Build();
        }

        private void Initialize()
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();
            var model = GetDataModel<Model>();

            if (dbPlayerTripleTriad.Decks.ContainsKey(dbPlayerTripleTriad.LastDeckId))
                model.SelectedDeckId = dbPlayerTripleTriad.LastDeckId;
        }

        private void MainPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var deckSelected = ColorToken.Yellow("[NO DECK SELECTED]");

            if (model.SelectedDeckId > 0)
            {
                var deck = dbPlayerTripleTriad.Decks[model.SelectedDeckId];
                deckSelected = string.IsNullOrWhiteSpace(deck.Name)
                    ? $"Deck #{model.SelectedDeckId}"
                    : deck.Name;
            }

            page.Header = "You can create or join Triple Triad games here. You can customize your decks and view your cards from the rest menu (press 'R') or by clicking 'View Cards' here.\n\n" +
                ColorToken.Green("Deck: ") + deckSelected;

            page.AddResponse(ColorToken.Green("Refresh"), () => { });
            page.AddResponse("View Cards", () => SwitchConversation(nameof(TripleTriadPlayerDialog)));

            // Player has a game reservation. Give them the option to close it.
            if (TripleTriad.DoesPlayerHavePendingGame(player))
            {
                if (model.IsConfirming)
                {
                    page.AddResponse("CONFIRM CANCEL RESERVATION", () =>
                    {
                        model.IsConfirming = false;

                        if (TripleTriad.DoesPlayerHavePendingGame(player))
                            TripleTriad.RemovePlayerPendingGame(player);

                        FloatingTextStringOnCreature("Game reservation has been cancelled.", player, false);
                    });
                }
                else
                {
                    page.AddResponse("Cancel Game Reservation", () =>
                    {
                        model.IsConfirming = true;
                    });
                }
            }
            // Otherwise let them create a new game or join someone else's game.
            else
            {
                page.AddResponse("Select Deck", () =>
                {
                    ChangePage(SelectDeckPageId);
                });

                if (model.SelectedDeckId > 0)
                {
                    var deck = dbPlayerTripleTriad.Decks[model.SelectedDeckId];
                    page.AddResponse("Create New Game", () =>
                    {
                        TripleTriad.CreatePendingGame(player, new CardPendingGame
                        {
                            Player1Deck = deck
                        });

                        EndConversation();
                        FloatingTextStringOnCreature("Game has been created. Please wait for another player to join. Leaving the area or server will remove your game reservation.", player, false);
                    });

                    foreach (var (gameOwner, pendingGame) in TripleTriad.GetAllPendingGames())
                    {
                        page.AddResponse($"Join Game: {GetName(gameOwner)}", () =>
                        {
                            if (!TripleTriad.DoesPlayerHavePendingGame(gameOwner) || // Game no longer exists
                                !GetIsObjectValid(gameOwner) ||                      // Game owner is no longer valid
                                GetTag(GetArea(gameOwner)) == "tt_arena")      // Game owner has entered a game
                            {
                                FloatingTextStringOnCreature("This game is no longer available.", player, false);
                                return;
                            }

                            EndConversation();
                            TripleTriad.StartGame(gameOwner, pendingGame.Player1Deck, player, deck); // todo: rule config
                        });
                    }
                }
            }
            
        }

        private void SelectDeckPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();
            var model = GetDataModel<Model>();
            page.Header = "Please select a deck from the list below. You can create decks from the rest menu (press 'R').\n\n";

            foreach (var (deckId, deck) in dbPlayerTripleTriad.Decks)
            {
                // Ensure the deck is valid.
                if (deck.Card1 != CardType.Invalid &&
                    deck.Card2 != CardType.Invalid &&
                    deck.Card3 != CardType.Invalid &&
                    deck.Card4 != CardType.Invalid &&
                    deck.Card5 != CardType.Invalid)
                {
                    page.AddResponse($"Select Deck: {deck.Name}", () =>
                    {
                        model.SelectedDeckId = deckId;
                        dbPlayerTripleTriad.LastDeckId = deckId;
                        DB.Set(playerId, dbPlayerTripleTriad);

                        ClearNavigationStack();
                        ChangePage(MainPageId, false);
                    });
                }
            }
        }

        /// <summary>
        /// If a player leaves an area or the module, remove their pending game from the list.
        /// </summary>
        [NWNEventHandler("mod_exit")]
        [NWNEventHandler("area_exit")]
        public static void ClearPendingGameOnExit()
        {
            var player = GetExitingObject();
            TripleTriad.RemovePlayerPendingGame(player);
        }

    }
}
