using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWNX.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Feature;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Location = NWN.FinalFantasy.Core.Location;
using Object = NWN.FinalFantasy.Core.NWNX.Object;

namespace NWN.FinalFantasy.Service
{
    public static class TripleTriad
    {
        // Default card graphic texture
        private const string DefaultCardTexture = "Card_Back";

        // Element texture
        private const string ElementTexture = "card_elem";

        // Determines the texture names used for each power slot
        private const string CardPowerPlayer1Top = "Card_Pwr_B_2";
        private const string CardPowerPlayer1Bottom = "Card_Pwr_B_3";
        private const string CardPowerPlayer1Left = "Card_Pwr_B_1";
        private const string CardPowerPlayer1Right = "Card_Pwr_B_4";

        private const string CardPowerPlayer2Top = "Card_Pwr_R_2";
        private const string CardPowerPlayer2Bottom = "Card_Pwr_R_3";
        private const string CardPowerPlayer2Left = "Card_Pwr_R_4";
        private const string CardPowerPlayer2Right = "Card_Pwr_R_1"; 

        // Texture used for the face-down cards on power textures
        private const string EmptyTexture = "Card_None";

        // Resref of the base arena area
        private const string ArenaResref = "tt_arena";

        // Resref of all card placeables
        private const string CardResref = "tt_card";

        // Determines how big to scale the cards placed on the board
        private const float BoardCardScale = 2.00f;

        // Determines how big to scale cards held in player hands
        private const float HandCardScale = 1.25f;

        // Resref of the selection shafts of light for each player
        private const string Player1SelectionResref = "plc_solblue";
        private const string Player2SelectionResref = "plc_solred";

        // Background textures
        private const string Player1BackgroundTexture = "Card_Bkg_Blue";
        private const string Player2BackgroundTexture = "Card_Bkg_Red";

        // Tracks all of the registered cards
        private static Dictionary<CardType, Card> AvailableCards { get; set; }
        private static Dictionary<int, List<CardType>> CardsByLevel { get; set; } = new Dictionary<int, List<CardType>>();

        // Tracks all of the active game states
        private static Dictionary<string, CardGameState> GameStates { get; set; } = new Dictionary<string, CardGameState>();

        // Tracks all of the pending games that players can join.
        private static readonly Dictionary<uint, CardPendingGame> _pendingGames = new Dictionary<uint, CardPendingGame>();

        /// <summary>
        /// When the module loads, all card data is cached and sorted for quick lookups.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void LoadCardData()
        {
            AvailableCards = CardList.Create();

            foreach (var (type, card) in AvailableCards)
            {
                if (!CardsByLevel.ContainsKey(card.Level))
                    CardsByLevel[card.Level] = new List<CardType>();

                CardsByLevel[card.Level].Add(type);
            }
        }

        /// <summary>
        /// Retrieves all available cards.
        /// </summary>
        /// <returns>All available cards</returns>
        public static Dictionary<CardType, Card> GetAllAvailableCards()
        {
            return AvailableCards.ToDictionary(x => x.Key, y => y.Value);
        }

        /// <summary>
        /// Retrieves all of the cards at a given level.
        /// </summary>
        /// <param name="level">The level to retrieve at.</param>
        /// <returns>A dictionary containing cards at a given level.</returns>
        public static Dictionary<CardType, Card> GetAllCardsAtLevel(int level)
        {
            if(!CardsByLevel.ContainsKey(level))
                return new Dictionary<CardType, Card>();

            var result = new Dictionary<CardType, Card>();
            var cardsAtLevel = CardsByLevel[level];

            foreach (var card in cardsAtLevel)
            {
                result.Add(card, AvailableCards[card]);
            }

            return result;
        }

        /// <summary>
        /// Retrieves a specific card by its type.
        /// </summary>
        /// <param name="cardType">The type of card to retrieve.</param>
        /// <returns>A card detail object matching the specified type.</returns>
        public static Card GetCardByType(CardType cardType)
        {
            return AvailableCards[cardType];
        }

        /// <summary>
        /// Returns true if player has an active pending game.
        /// </summary>
        /// <param name="player">The player to check</param>
        /// <returns>true if player has a pending game, false otherwise</returns>
        public static bool DoesPlayerHavePendingGame(uint player)
        {
            return _pendingGames.ContainsKey(player);
        }

        /// <summary>
        /// Removes a player's pending game.
        /// </summary>
        /// <param name="player">The player whose pending game will be removed</param>
        public static void RemovePlayerPendingGame(uint player)
        {
            if (!DoesPlayerHavePendingGame(player)) return;

            _pendingGames.Remove(player);
        }

        /// <summary>
        /// Creates a new pending game associated with a player.
        /// </summary>
        /// <param name="player">The player creating the pending game.</param>
        /// <param name="pendingGame">The pending game details</param>
        public static void CreatePendingGame(uint player, CardPendingGame pendingGame)
        {
            _pendingGames[player] = pendingGame;
        }

        /// <summary>
        /// Returns all of the pending games available to select from.
        /// </summary>
        /// <returns>A dictionary containing all pending games.</returns>
        public static Dictionary<uint, CardPendingGame> GetAllPendingGames()
        {
            return _pendingGames.ToDictionary(x => x.Key, y => y.Value);
        }

        /// <summary>
        /// Creates an arena instance and stores all necessary locations on the instance.
        /// </summary>
        /// <returns>The instanced arena area.</returns>
        private static uint CreateArena()
        {
            Location GetInstanceLocation(uint instanceArea, Location originalLocation)
            {
                var position = GetPositionFromLocation(originalLocation);
                var facing = GetFacingFromLocation(originalLocation);
                var instanceLocation = Location(instanceArea, position, facing);

                return instanceLocation;
            }

            // Copy the original arena area.
            var original = Cache.GetAreaByResref(ArenaResref);
            var instance = CopyArea(original);
            SetEventScript(instance, EventScript.Area_OnHeartbeat, "tt_area_hb");
            
            // Store player start locations
            SetLocalLocation(instance, "PLAYER_1_START", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_START"))));
            SetLocalLocation(instance, "PLAYER_2_START", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_START"))));

            // Store player 1 hand locations
            SetLocalLocation(instance, "PLAYER_1_HAND_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_1"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_2"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_3", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_3"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_4", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_4"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_5", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_5"))));

            // Store player 2 hand locations
            SetLocalLocation(instance, "PLAYER_2_HAND_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_1"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_2"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_3", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_3"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_4", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_4"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_5", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_5"))));

            // Store score locations
            SetLocalLocation(instance, "PLAYER_1_SCORE", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_SCORE"))));
            SetLocalLocation(instance, "PLAYER_2_SCORE", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_SCORE"))));

            // Store board locations
            SetLocalLocation(instance, "BOARD_0_0", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_0_0"))));
            SetLocalLocation(instance, "BOARD_0_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_0_1"))));
            SetLocalLocation(instance, "BOARD_0_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_0_2"))));
            SetLocalLocation(instance, "BOARD_1_0", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_1_0"))));
            SetLocalLocation(instance, "BOARD_1_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_1_1"))));
            SetLocalLocation(instance, "BOARD_1_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_1_2"))));
            SetLocalLocation(instance, "BOARD_2_0", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_2_0"))));
            SetLocalLocation(instance, "BOARD_2_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_2_1"))));
            SetLocalLocation(instance, "BOARD_2_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_2_2"))));

            return instance;
        }

        /// <summary>
        /// Retrieves the game Id from an arena area.
        /// </summary>
        /// <param name="arenaArea">The arena area</param>
        /// <returns>A string representing the game state Id</returns>
        private static string GetGameId(uint arenaArea)
        {
            return GetLocalString(arenaArea, "TRIPLE_TRIAD_GAME_ID");
        }

        /// <summary>
        /// Retrieves the player and slot of a given placeable card.
        /// Only use this on card placeables in hands, not on the board.
        /// </summary>
        /// <param name="placeable">The hand card placeable</param>
        /// <returns>The card game player and the index of the card in the hand.</returns>
        private static (CardPlayerType, int) GetCardHandDetails(uint placeable)
        {
            var playerNumber = GetLocalInt(placeable, "TRIPLE_TRIAD_CARD_OWNER");
            var cardGamePlayer = playerNumber == 1 ? CardPlayerType.Player1 : CardPlayerType.Player2;
            var handSlot = GetLocalInt(placeable, "TRIPLE_TRIAD_CARD_HAND_SLOT");

            return (cardGamePlayer, handSlot);
        }

        /// <summary>
        /// Starts a new Triple Triad game. At least one player must be a PC or the system won't function properly.
        /// </summary>
        /// <param name="player1">The first player</param>
        /// <param name="player1Deck">The first player's deck.</param>
        /// <param name="player2">The second player</param>
        /// <param name="player2Deck">The second player's deck.</param>
        /// <param name="rules">The set of rules to use for this game.</param>
        public static void StartGame(
            uint player1, 
            CardDeck player1Deck,
            uint player2,
            CardDeck player2Deck,
            CardRuleType rules = CardRuleType.None)
        {
            uint WarpOrCloneIntoArena(uint player, uint area, string locationId)
            {
                var playerStartLocation = GetLocalLocation(area, locationId);
                // Players get warped into the arena.
                if (GetIsPC(player))
                {
                    SetLocalLocation(player, "TRIPLE_TRIAD_WARP_POINT", GetLocation(player));

                    AssignCommand(player, () => ActionJumpToLocation(playerStartLocation));
                }
                // NPCs are cloned in the arena.
                else
                {
                    player = CopyObject(player, playerStartLocation);
                    SetImmortal(player, true);

                    SetEventScript(player, EventScript.Creature_OnHeartbeat, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnNotice, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnSpellCastAt, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnMeleeAttacked, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnDamaged, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnDisturbed, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnEndCombatRound, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnDialogue, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnSpawnIn, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnRested, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnDeath, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnUserDefined, string.Empty);
                    SetEventScript(player, EventScript.Creature_OnBlockedByDoor, string.Empty);
                }

                return player;
            }

            var arenaArea = CreateArena();

            player1 = WarpOrCloneIntoArena(player1, arenaArea, "PLAYER_1_START");
            player2 = WarpOrCloneIntoArena(player2, arenaArea, "PLAYER_2_START");

            // Register the game state
            var state = new CardGameState(arenaArea, player1, player2);
            var gameId = Guid.NewGuid().ToString();
            GameStates[gameId] = state;
            SetLocalString(arenaArea, "TRIPLE_TRIAD_GAME_ID", gameId);

            state.Rules = rules;

            // Register player 1's hand
            state.Players[CardPlayerType.Player1].Hand[1].Type = player1Deck.Card1;
            state.Players[CardPlayerType.Player1].Hand[2].Type = player1Deck.Card2;
            state.Players[CardPlayerType.Player1].Hand[3].Type = player1Deck.Card3;
            state.Players[CardPlayerType.Player1].Hand[4].Type = player1Deck.Card4;
            state.Players[CardPlayerType.Player1].Hand[5].Type = player1Deck.Card5;

            // Register player 2's hand
            state.Players[CardPlayerType.Player2].Hand[1].Type = player2Deck.Card1;
            state.Players[CardPlayerType.Player2].Hand[2].Type = player2Deck.Card2;
            state.Players[CardPlayerType.Player2].Hand[3].Type = player2Deck.Card3;
            state.Players[CardPlayerType.Player2].Hand[4].Type = player2Deck.Card4;
            state.Players[CardPlayerType.Player2].Hand[5].Type = player2Deck.Card5;
        }

        /// <summary>
        /// Handles the game's logic processing.
        /// This handles initialization of the game, handling idle players, and other checks which must happen.
        /// </summary>
        [NWNEventHandler("tt_area_hb")]
        public static void GameLogicProcessing()
        {
            var area = OBJECT_SELF;
            var gameId = GetGameId(area);

            // Only Triple Triad arena areas with active games should be processed.
            if (string.IsNullOrWhiteSpace(gameId)) return;

            var state = GameStates[gameId];

            // Game hasn't initialized yet.
            if (!state.HasInitialized)
            {
                InitializeGame(gameId);
            }
            else if(state.HasInitialized)
            {
                // Check if the game needs to end. If it does, don't bother running any other logic.
                var isEnding = ProcessEndGameLogic(gameId);
                if (isEnding) return;

                // Otherwise process NPC AI, if applicable to this game
                ProcessNPCLogic(gameId, CardPlayerType.Player1);
                ProcessNPCLogic(gameId, CardPlayerType.Player2);
            }
        }

        /// <summary>
        /// Initializes the game by spawning each player's hands 
        /// </summary>
        /// <param name="gameId">The game's state Id</param>
        private static void InitializeGame(string gameId)
        {
            var state = GameStates[gameId];

            // Both players must physically be in the arena to initialize.
            if (GetArea(state.Players[CardPlayerType.Player1].Player) != state.ArenaArea ||
                GetArea(state.Players[CardPlayerType.Player2].Player) != state.ArenaArea)
                return;

            foreach (var (type, player) in state.Players)
            {
                var playerNumber = type == CardPlayerType.Player1 ? 1 : 2;
                var animation = type == CardPlayerType.Player1 ? Animation.PlaceableDeactivate : Animation.PlaceableActivate;

                // Spawn player's hand
                for (var index = 1; index <= 5; index++)
                {
                    var cardType = player.Hand[index].Type;
                    var placeable = SpawnCard(gameId, $"PLAYER_{playerNumber}_HAND_{index}", cardType, HandCardScale);
                    player.Hand[index].Placeable = placeable;
                    player.Hand[index].Type = cardType;
                    SetLocalInt(placeable, "TRIPLE_TRIAD_CARD_HAND_SLOT", index);
                    SetLocalInt(placeable, "TRIPLE_TRIAD_CARD_OWNER", playerNumber);
                    SetEventScript(placeable, EventScript.Placeable_OnLeftClick, "tt_card_select");
                    AssignCommand(placeable, () => ActionPlayAnimation(animation));
                }
            }

            // Spawn blank cards on the board
            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    var locationId = $"BOARD_{x}_{y}";
                    var placeable = SpawnCard(gameId, locationId, CardType.Invalid);
                    SetName(placeable, $"({x}, {y})");
                    SetEventScript(placeable, EventScript.Placeable_OnLeftClick, "tt_card_place");
                    SetLocalInt(placeable, "TRIPLE_TRIAD_X", x);
                    SetLocalInt(placeable, "TRIPLE_TRIAD_Y", y);

                    state.Board[x, y].Placeable = placeable;
                    ReplaceObjectTexture(placeable, Player2BackgroundTexture, EmptyTexture);
                    ReplaceObjectTexture(placeable, "Card_Board", EmptyTexture);
                }
            }

            // Spawn score cards
            state.Player1ScorePlaceable = SpawnCard(gameId, "PLAYER_1_SCORE", CardType.Invalid, 0.8f);
            SetUseableFlag(state.Player1ScorePlaceable, false);
            ReplaceObjectTexture(state.Player1ScorePlaceable, DefaultCardTexture, GetPowerTexture(5));
            ReplaceObjectTexture(state.Player1ScorePlaceable, Player1BackgroundTexture, EmptyTexture);
            ReplaceObjectTexture(state.Player1ScorePlaceable, Player2BackgroundTexture, EmptyTexture);

            state.Player2ScorePlaceable = SpawnCard(gameId, "PLAYER_2_SCORE", CardType.Invalid, 0.8f);
            SetUseableFlag(state.Player2ScorePlaceable, false);
            ReplaceObjectTexture(state.Player2ScorePlaceable, DefaultCardTexture, GetPowerTexture(5));
            ReplaceObjectTexture(state.Player2ScorePlaceable, Player1BackgroundTexture, EmptyTexture);
            ReplaceObjectTexture(state.Player2ScorePlaceable, Player2BackgroundTexture, EmptyTexture);

            if (Random.D100(1) <= 50)
            {
                state.CurrentPlayerTurn = CardPlayerType.Player1;
                SendMessageToAllPlayers(gameId, $"{GetName(state.Players[CardPlayerType.Player1].Player)} won the coin toss and goes first.");
            }
            else
            {
                state.CurrentPlayerTurn = CardPlayerType.Player2;
                SendMessageToAllPlayers(gameId, $"{GetName(state.Players[CardPlayerType.Player2].Player)} won the coin toss and goes first.");
            }

            state.HasInitialized = true;
        }

        private static void SendMessageToAllPlayers(string gameId, string message)
        {
            var state = GameStates[gameId];

            foreach (var player in state.Players)
            {
                SendMessageToPC(player.Value.Player, message);
            }
        }

        /// <summary>
        /// Processes the "End Game" logic used during processing.
        /// </summary>
        /// <param name="gameId">The game's state Id</param>
        /// <returns>true if the game is ending, false otherwise</returns>
        private static bool ProcessEndGameLogic(string gameId)
        {
            var state = GameStates[gameId];
            if (state.IsGameCleaningUp) return true;

            // Is game ending normally?
            if (state.IsGameEnding)
            {
                state.GameEndingTicks++;
                SendMessageToAllPlayers(gameId, "Game will be ending soon...");

                if (state.GameEndingTicks >= 3) // Roughly 18 seconds after ending, the game will clean up.
                {
                    foreach (var (_, value) in state.Players)
                    {
                        var player = value.Player;

                        // Send the players back to their saved locations if they're still in the arena.
                        if (GetIsPC(player))
                        {
                            AssignCommand(player, () =>
                            {
                                ClearAllActions();
                                ActionJumpToLocation(GetLocalLocation(value.Player, "TRIPLE_TRIAD_WARP_POINT"));
                            });
                        }
                        // NPCs get destroyed instead.
                        else
                        {
                            DestroyObject(player);
                        }
                    }

                    // Clear game state and area.
                    DelayCommand(30f, () =>
                    {
                        DestroyArea(state.ArenaArea);
                        GameStates.Remove(gameId);
                    });

                    state.IsGameCleaningUp = true;
                }

                return true;
            }
            // Otherwise handle player disconnection logic.
            else
            {
                // One or more player has left the area or disconnected.
                if (GetArea(state.Players[CardPlayerType.Player1].Player) != state.ArenaArea ||
                    GetArea(state.Players[CardPlayerType.Player2].Player) != state.ArenaArea)
                {
                    state.DisconnectionCheckCounter++;
                }
                // Both players are in the arena, reset the counter.
                else
                {
                    state.DisconnectionCheckCounter = 0;
                }

                // Counter has reached limit. End the game prematurely.
                if (state.DisconnectionCheckCounter >= 10)
                {
                    SendMessageToAllPlayers(gameId, "One or more players have left the Triple Triad arena. The game will end now.");
                    EndGame(gameId, true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Processes NPC AI logic to simulate playing.
        /// </summary>
        /// <param name="gameId">The game's state Id</param>
        /// <param name="playerNumber">Which player number is associated with the NPC we want to process.</param>
        private static void ProcessNPCLogic(string gameId, CardPlayerType playerNumber)
        {
            var state = GameStates[gameId];
            var playerData = state.Players[playerNumber];
            var player = playerData.Player;

            if (GetIsPC(player)) return; // Not an NPC, so exit early.
            if (state.CurrentPlayerTurn != playerNumber) return; // Not the NPC's turn, skip for now.

            // NPC player makes a decision roughly every 12 seconds.
            playerData.DecisionCounter++;
            if (playerData.DecisionCounter < 2) return;

            // Attempt to determine the best play.
            // Will not find a play if no cards are on the board or if none of player's cards will be opponent's cards.
            var (handCardId, x, y) = DetermineBestPlay(gameId, playerNumber);

            // Best play couldn't be determined. Pick the best corner play.
            // Will not find a play if a card exists in all four corners.
            if (handCardId <= -1)
            {
                (handCardId, x, y) = DetermineCornerPlay(gameId, playerNumber);
            }

            // Best play and Corner play couldn't be determined. Pick a random location.
            if (handCardId <= -1)
            {
                (handCardId, x, y) = DetermineRandomPlay(gameId, playerNumber);
            }

            var handCard = playerData.Hand[handCardId];
            var card = AvailableCards[handCard.Type];

            // Update the selected card.
            state.Players[playerNumber].CardSelection.CardHandId = handCardId;
            DoCardPlacement(gameId, x, y);

            SendMessageToAllPlayers(gameId, $"{GetName(player)} plays '{card.Name}' at position ({x}, {y}).");
            playerData.DecisionCounter = 0;
        }

        /// <summary>
        /// Determines the best play to make based on the player's hand and board state.
        /// If a best play can't be determined, all return values will be -1.
        /// </summary>
        /// <param name="gameId">The game's Id</param>
        /// <param name="playerNumber">The player to make the play determination for.</param>
        /// <returns>A hand card Id, board's X position, and board's Y position. All values will be -1 if play cannot be determined.</returns>
        private static (int, int, int) DetermineBestPlay(string gameId, CardPlayerType playerNumber)
        {
            var state = GameStates[gameId];
            var hand = state.Players[playerNumber].Hand;

            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    foreach (var (index, handCard) in hand)
                    {
                        var card = AvailableCards[handCard.Type];

                        // Compare card's LEFT rank with opponent's RIGHT power
                        if (y != 0)
                        {
                            var boardCard = state.Board[x, y - 1];
                            var opponentCard = AvailableCards[boardCard.CardType];
                            if (state.Board[x, y - 1].CurrentOwner != playerNumber &&   // Adjacent card must be owned by opponent.
                                state.Board[x, y - 1].CardType != CardType.Invalid &&     // Adjacent card must exist.
                                state.Board[x, y].CardType == CardType.Invalid &&       // Potential location must not already have a card.
                                card.LeftPower > opponentCard.RightPower)               // Power check
                            {
                                //Console.WriteLine($"Picked: {card.Name} at ({x}, {y})   (LEFT vs RIGHT)");
                                //Console.WriteLine($"Against: {opponentCard.Name} [{card.LeftPower} vs {opponentCard.RightPower}]");
                                return (index, x, y);
                            }
                        }

                        // Compare card's BOTTOM rank with oponent's TOP power
                        if (x != 2)
                        {
                            var boardCard = state.Board[x + 1, y];
                            var opponentCard = AvailableCards[boardCard.CardType];
                            if (state.Board[x + 1, y].CurrentOwner != playerNumber &&   // Adjacent card must be owned by opponent.
                                state.Board[x + 1, y].CardType != CardType.Invalid &&   // Adjacent card must exist.
                                state.Board[x, y].CardType == CardType.Invalid &&       // Potential location must not already have a card.
                                card.BottomPower > opponentCard.TopPower)               // Power check
                            {
                                //Console.WriteLine($"Picked: {card.Name} at ({x}, {y})   (BOTTOM vs TOP)");
                                //Console.WriteLine($"Against: {opponentCard.Name} [{card.BottomPower} vs {opponentCard.TopPower}]");
                                return (index, x, y);
                            }
                        }

                        // Compare card's RIGHT rank with opponent's LEFT power
                        if (y != 2)
                        {
                            var boardCard = state.Board[x, y + 1];
                            var opponentCard = AvailableCards[boardCard.CardType];
                            if (state.Board[x, y + 1].CurrentOwner != playerNumber &&   // Adjacent card must be owned by opponent.
                                state.Board[x, y + 1].CardType != CardType.Invalid &&   // Adjacent card must exist.
                                state.Board[x, y].CardType == CardType.Invalid &&       // Potential location must not already have a card.
                                card.RightPower > opponentCard.LeftPower)               // Power check
                            {
                                //Console.WriteLine($"Picked: {card.Name} at ({x}, {y})   (RIGHT vs LEFT)");
                                //Console.WriteLine($"Against: {opponentCard.Name} [{card.RightPower} vs {opponentCard.LeftPower}]");
                                return (index, x, y);
                            }
                        }

                        // Compare card's TOP rank with the opponent's BOTTOM power
                        if (x != 0)
                        {
                            var boardCard = state.Board[x - 1, y];
                            var opponentCard = AvailableCards[boardCard.CardType];
                            if (state.Board[x - 1, y].CurrentOwner != playerNumber &&   // Adjacent card must be owned by opponent.
                                state.Board[x - 1, y].CardType != CardType.Invalid &&   // Adjacent card must exist.
                                state.Board[x, y].CardType == CardType.Invalid &&       // Potential location must not already have a card.
                                card.TopPower > opponentCard.BottomPower)               // Power check
                            {
                                //Console.WriteLine($"Picked: {card.Name} at ({x}, {y})   (TOP vs BOTTOM)");
                                //Console.WriteLine($"Against: {opponentCard.Name} [{card.TopPower} vs {opponentCard.BottomPower}]");
                                return (index, x, y);
                            }
                        }
                    }
                }
            }

            return (-1, -1, -1);
        }

        /// <summary>
        /// Determines the corner play to make based on the player's highest powered cards in their hand.
        /// If all corners are occupied, no play will be determined.
        /// </summary>
        /// <param name="gameId">The game's Id</param>
        /// <param name="playerNumber">The player to determine the play for</param>
        /// <returns>The handId, board X position, and board Y position.</returns>
        private static (int, int, int) DetermineCornerPlay(string gameId, CardPlayerType playerNumber)
        {
            var state = GameStates[gameId];
            var corners = new List<Tuple<int, int>>();
            if (state.Board[0, 0].CardType == CardType.Invalid)
                corners.Add(new Tuple<int, int>(0, 0));
            if (state.Board[0, 2].CardType == CardType.Invalid)
                corners.Add(new Tuple<int, int>(0, 2));
            if (state.Board[2, 0].CardType == CardType.Invalid)
                corners.Add(new Tuple<int, int>(2, 0));
            if (state.Board[2, 2].CardType == CardType.Invalid)
                corners.Add(new Tuple<int, int>(2, 2));

            // No corners are available.
            if (corners.Count <= 0)
                return (-1, -1, -1);

            // Pick a random corner
            var (x, y) = corners.ElementAt(Random.Next(corners.Count));
            var handId = 0;

            // (0, 0) - Get highest right and bottom
            if (x == 0 && y == 0)
            {
                var highestAmount = 0;
                
                foreach (var handCard in state.Players[playerNumber].Hand)
                {
                    var card = AvailableCards[handCard.Value.Type];
                    if (card.RightPower + card.BottomPower > highestAmount)
                    {
                        handId = handCard.Key;
                    }
                }
            }
            // (0, 2) - Get highest left and bottom
            else if (x == 0 && y == 2)
            {
                var highestAmount = 0;

                foreach (var handCard in state.Players[playerNumber].Hand)
                {
                    var card = AvailableCards[handCard.Value.Type];
                    if (card.LeftPower + card.BottomPower > highestAmount)
                    {
                        handId = handCard.Key;
                    }
                }
            }
            // (2, 0) - Get highest right and top
            else if (x == 2 && y == 0)
            {
                var highestAmount = 0;

                foreach (var handCard in state.Players[playerNumber].Hand)
                {
                    var card = AvailableCards[handCard.Value.Type];
                    if (card.RightPower + card.TopPower > highestAmount)
                    {
                        handId = handCard.Key;
                    }
                }
            }
            // (2, 2) - Get highest left and top
            else if (x == 2 && y == 2)
            {
                var highestAmount = 0;

                foreach (var handCard in state.Players[playerNumber].Hand)
                {
                    var card = AvailableCards[handCard.Value.Type];
                    if (card.LeftPower + card.TopPower > highestAmount)
                    {
                        handId = handCard.Key;
                    }
                }
            }

            //Console.WriteLine($"Picked: {AvailableCards[state.Players[playerNumber].Hand[handId].Type].Name} at ({x}, {y})");

            return (handId, x, y);
        }

        /// <summary>
        /// Randomly select a card and location on the board to play.
        /// </summary>
        /// <param name="gameId">The game's Id</param>
        /// <param name="playerNumber">The player to make the play for</param>
        /// <returns>A hand card Id, board's X position, and board's Y position</returns>
        private static (int, int, int) DetermineRandomPlay(string gameId, CardPlayerType playerNumber)
        {
            var state = GameStates[gameId];
            var hand = state.Players[playerNumber].Hand;

            // Pick a random card to play.
            var cardToPlay = hand.ElementAt(Random.Next(hand.Count));

            // Identify the open board positions.
            var availableSlots = new List<Tuple<int, int>>();

            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    if (state.Board[x, y].CardType == CardType.Invalid)
                    {
                        availableSlots.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            // Pick a position to play at randomly.
            var index = Random.Next(availableSlots.Count - 1);
            var (boardX, boardY) = availableSlots[index];

            //Console.WriteLine($"Random play: {cardToPlay.Key}, ({boardX}, {boardY})");

            return (cardToPlay.Key, boardX, boardY);
        }

        private static void ChangeTurn(string gameId, CardPlayerType playerTurn)
        {
            var state = GameStates[gameId];
            state.CurrentPlayerTurn = playerTurn;

            if (playerTurn == CardPlayerType.Player1)
            {
                var message = $"It is {GetName(state.Players[CardPlayerType.Player1].Player)}'s turn.";
                SendMessageToAllPlayers(gameId, message);
            }
            else
            {
                var message = $"It is {GetName(state.Players[CardPlayerType.Player2].Player)}'s turn.";
                SendMessageToAllPlayers(gameId, message);
            }
        }

        /// <summary>
        /// Spawns a card at a specific location Id.
        /// </summary>
        /// <param name="gameId">The game's state Id</param>
        /// <param name="locationId">The location Id</param>
        /// <param name="cardType">The type of card to spawn</param>
        /// <param name="scale">Scale of the card placeable</param>
        /// <returns>The placeable spawned.</returns>
        private static uint SpawnCard(string gameId, string locationId, CardType cardType, float scale = BoardCardScale)
        {
            var state = GameStates[gameId];
            var area = state.ArenaArea;
            var card = AvailableCards[cardType];

            var location = GetLocalLocation(area, locationId);
            var placeable = CreateObject(ObjectType.Placeable, CardResref, location);
            SetName(placeable, card.Name);

            // Set the card graphic
            ReplaceObjectTexture(placeable, DefaultCardTexture, card.Texture);

            // Set power levels
            ReplaceObjectTexture(placeable, CardPowerPlayer1Right, GetPowerTexture(card.RightPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer1Top, GetPowerTexture(card.TopPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer1Bottom, GetPowerTexture(card.BottomPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer1Left, GetPowerTexture(card.LeftPower));

            ReplaceObjectTexture(placeable, CardPowerPlayer2Right, GetPowerTexture(card.RightPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer2Top, GetPowerTexture(card.TopPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer2Bottom, GetPowerTexture(card.BottomPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer2Left, GetPowerTexture(card.LeftPower));

            // Set the element texture
            ReplaceObjectTexture(placeable, ElementTexture, GetElementTexture(card.Element));

            // Scale the card to fit the board
            SetObjectVisualTransform(placeable, ObjectVisualTransform.Scale, scale);

            return placeable;
        }

        /// <summary>
        /// Retrieves the texture used for a specific power level.
        /// Returns an empty texture if value is outside range of 0-10
        /// </summary>
        /// <param name="power">The power level</param>
        /// <returns>A texture with a matching power level.</returns>
        private static string GetPowerTexture(int power)
        {
            if (power >= 0 && power <= 10)
            {
                return $"Card_{power}";
            }

            return EmptyTexture;
        }

        private static void FlipBoardCard(string gameId, uint placeable)
        {
            var state = GameStates[gameId];
            var x = GetLocalInt(placeable, "TRIPLE_TRIAD_X");
            var y = GetLocalInt(placeable, "TRIPLE_TRIAD_Y");
            var boardCard = state.Board[x, y];

            if (boardCard.CurrentOwner == CardPlayerType.Player1)
            {
                AssignCommand(boardCard.Placeable, () => ActionPlayAnimation(Animation.PlaceableDeactivate));
            }
            else if (boardCard.CurrentOwner == CardPlayerType.Player2)
            {
                AssignCommand(boardCard.Placeable, () => ActionPlayAnimation(Animation.PlaceableActivate));
            }
        }

        /// <summary>
        /// Retrieves the texture associated with a card element type.
        /// </summary>
        /// <param name="elementType">The card's element type</param>
        /// <returns>A texture string matching the specified element type.</returns>
        public static string GetElementTexture(CardElementType elementType)
        {
            switch (elementType)
            {
                case CardElementType.Earth:
                    return "Card_Ele_Earth";
                case CardElementType.Fire:
                    return "Card_Ele_Fire";
                case CardElementType.Water:
                    return "Card_Ele_Water";
                case CardElementType.Poison:
                    return "Card_Ele_Poison";
                case CardElementType.Holy:
                    return "Card_Ele_Holy";
                case CardElementType.Lightning:
                    return "Card_Ele_Lgtng";
                case CardElementType.Wind:
                    return "Card_Ele_Wind";
                case CardElementType.Ice:
                    return "Card_Ele_Ice";
            }

            return EmptyTexture;
        }

        /// <summary>
        /// When a card is clicked, select the card.
        /// </summary>
        [NWNEventHandler("tt_card_select")]
        public static void ClickCard()
        {
            var player = GetPlaceableLastClickedBy();
            var placeable = OBJECT_SELF;
            var area = GetArea(placeable);
            var (owner, cardHandId) = GetCardHandDetails(placeable);
            var gameId = GetGameId(area);
            var state = GameStates[gameId];

            AssignCommand(player, () => ClearAllActions());

            if (player != state.Players[CardPlayerType.Player1].Player && owner == CardPlayerType.Player1 ||
                player != state.Players[CardPlayerType.Player2].Player && owner == CardPlayerType.Player2)
            {
                SendMessageToPC(player, "You do not own that card.");
                return;
            }

            var selection = state.Players[owner].CardSelection;
            var resref = owner == CardPlayerType.Player1 ? Player1SelectionResref : Player2SelectionResref;
            var opponentType = owner == CardPlayerType.Player1 ? CardPlayerType.Player2 : CardPlayerType.Player1;

            if (selection.Placeable != null)
            {
                Object.SetPosition((uint)selection.Placeable, GetPosition(placeable));
            }
            else
            {
                var selectionPlaceable = CreateObject(ObjectType.Placeable, resref, GetLocation(placeable));
                selection.Placeable = selectionPlaceable;

                // Do this check just in case the player is playing against himself.
                if (!state.IsPlayingSelf)
                {
                    Visibility.SetVisibilityOverride(state.Players[owner].Player, selectionPlaceable, VisibilityType.Visible);
                    Visibility.SetVisibilityOverride(state.Players[opponentType].Player, selectionPlaceable, VisibilityType.Hidden);
                }
            }

            state.Players[owner].CardSelection.CardHandId = cardHandId;
            var card = AvailableCards[state.Players[owner].Hand[cardHandId].Type];
            SendMessageToPC(player, $"{card.Name}: [Top = {card.TopPower}, Bottom = {card.BottomPower}, Left = {card.LeftPower}, Right = {card.RightPower}]");
        }

        /// <summary>
        /// When a player clicks on a card on the board, attempt to place the selected card.
        /// </summary>
        [NWNEventHandler("tt_card_place")]
        public static void PlaceCard()
        {
            var player = GetPlaceableLastClickedBy();
            var placeable = OBJECT_SELF;
            var area = GetArea(placeable);
            var gameId = GetGameId(area);
            var state = GameStates[gameId];
            var x = GetLocalInt(placeable, "TRIPLE_TRIAD_X");
            var y = GetLocalInt(placeable, "TRIPLE_TRIAD_Y");

            AssignCommand(player, () => ClearAllActions());

            // Is it this the player's turn?
            if (state.CurrentPlayerTurn == CardPlayerType.Player1 &&
                state.Players[CardPlayerType.Player1].Player != player)
            {
                SendMessageToPC(player, "It is currently player 2's turn. Please wait your turn.");
                return;
            }
            else if (state.CurrentPlayerTurn == CardPlayerType.Player2 &&
                state.Players[CardPlayerType.Player2].Player != player)
            {
                SendMessageToPC(player, "It is currently player 1's turn. Please wait your turn.");
                return;
            }

            var selection = state.Players[state.CurrentPlayerTurn].CardSelection;

            // Player hasn't selected a card yet.
            if (selection.Placeable == null)
            {
                SendMessageToPC(player, "Select a card from your hand first.");
                return;
            }

            // Sanity check to make sure the player hasn't picked a location that already has a card.
            if (state.Board[x, y].CardType != CardType.Invalid)
            {
                return;
            }

            DoCardPlacement(gameId, x, y);
        }

        private static void DoCardPlacement(string gameId, int x, int y)
        {
            var state = GameStates[gameId];
            var selection = state.Players[state.CurrentPlayerTurn].CardSelection;

            // We've got a valid location. Update the selected card in this position, remove the card from the player's hand, and run game rules on this change.
            var hand = state.Players[state.CurrentPlayerTurn].Hand;

            // Remove the card from the player's hand
            var handCard = hand[selection.CardHandId];
            if (handCard.Placeable != null)
                DestroyObject((uint)handCard.Placeable);

            // Destroy the selection placeable
            if (selection.Placeable != null)
            {
                DestroyObject((uint)selection.Placeable);
                selection.Placeable = null;
            }

            // Destroy the blank card on the board
            DestroyObject(state.Board[x, y].Placeable);

            // Spawn a new card onto the board in the same position.
            var placeable = SpawnCard(gameId, $"BOARD_{x}_{y}", handCard.Type);
            SetUseableFlag(placeable, false);
            SetLocalInt(placeable, "TRIPLE_TRIAD_X", x);
            SetLocalInt(placeable, "TRIPLE_TRIAD_Y", y);

            // Update the board state
            state.Board[x, y].Placeable = placeable;
            state.Board[x, y].CardType = handCard.Type;
            state.Board[x, y].CurrentOwner = state.CurrentPlayerTurn;

            // Handle flipping the card to match the owner.
            FlipBoardCard(gameId, placeable);

            // Remove the card from the player's hand.
            hand.Remove(selection.CardHandId);

            // Change turns
            var nextTurn = state.CurrentPlayerTurn == CardPlayerType.Player1
                ? CardPlayerType.Player2
                : CardPlayerType.Player1;

            // Handle card fighting
            CardFight(gameId, x, y);

            // Have all cards been placed?
            if (CheckForEndCondition(gameId))
            {
                EndGame(gameId, false);
            }
            // Game is still going, change turns.
            else
            {
                ChangeTurn(gameId, nextTurn);
            }
        }

        /// <summary>
        /// Handles fighting cards in adjacent positions.
        /// </summary>
        /// <param name="gameId">The game Id</param>
        /// <param name="x">The X position of the card that was just placed</param>
        /// <param name="y">The Y position of the card that was just placed</param>
        private static void CardFight(string gameId, int x, int y)
        {
            var state = GameStates[gameId];

            int DeterminePower(CardType cardType, CardDirection direction)
            {
                var card = AvailableCards[cardType];

                switch (direction)
                {
                    case CardDirection.Top:
                        return card.TopPower;
                    case CardDirection.Bottom:
                        return card.BottomPower;
                    case CardDirection.Left:
                        return card.LeftPower;
                    case CardDirection.Right:
                        return card.RightPower;
                }

                return 0;
            }

            void DoFight(
                CardBoardPosition attacker, 
                CardBoardPosition defender, 
                CardDirection attackerDirection, 
                CardDirection defenderDirection)
            {
                // No defender card. Skip out of this as there's nothing to fight.
                if (defender.CardType == CardType.Invalid) return;
                // Same owner, no sense running the logic.
                if (attacker.CurrentOwner == defender.CurrentOwner) return;

                var attackerPower = DeterminePower(attacker.CardType, attackerDirection);
                var defenderPower = DeterminePower(defender.CardType, defenderDirection);

                if (attackerPower > defenderPower)
                {
                    defender.CurrentOwner = attacker.CurrentOwner;
                }

                // "Same" rule - If ranks are the same, attacker gains ownership
                if (state.Rules.HasFlag(CardRuleType.Same))
                {
                    if (attackerPower == defenderPower)
                    {
                        defender.CurrentOwner = attacker.CurrentOwner;
                    }
                }

                // todo: "Combo" rule would go here.

                FlipBoardCard(gameId, attacker.Placeable);
                FlipBoardCard(gameId, defender.Placeable);
            }

            var boardCard = state.Board[x, y];

            // Row 1
            if (x == 0 && y == 0)
            {
                DoFight(boardCard, state.Board[0, 1], CardDirection.Right, CardDirection.Left);
                DoFight(boardCard, state.Board[1, 0], CardDirection.Bottom, CardDirection.Top);
            }
            else if (x == 0 && y == 1)
            {
                DoFight(boardCard, state.Board[0, 0], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[0, 2], CardDirection.Right, CardDirection.Left);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Bottom, CardDirection.Top);
            }
            else if (x == 0 && y == 2)
            {
                DoFight(boardCard, state.Board[0, 1], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[1, 2], CardDirection.Bottom, CardDirection.Top);
            }

            // Row 2
            else if (x == 1 && y == 0)
            {
                DoFight(boardCard, state.Board[0, 0], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Right, CardDirection.Left);
                DoFight(boardCard, state.Board[2, 0], CardDirection.Bottom, CardDirection.Top);
            }
            else if (x == 1 && y == 1)
            {
                DoFight(boardCard, state.Board[0, 1], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[1, 0], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[2, 1], CardDirection.Bottom, CardDirection.Top);
                DoFight(boardCard, state.Board[1, 2], CardDirection.Right, CardDirection.Left);
            }
            else if (x == 1 && y == 2)
            {
                DoFight(boardCard, state.Board[0, 2], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[2, 2], CardDirection.Bottom, CardDirection.Top);
            }

            // Row 3
            else if (x == 2 && y == 0)
            {
                DoFight(boardCard, state.Board[1, 0], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[2, 1], CardDirection.Right, CardDirection.Left);
            }
            else if (x == 2 && y == 1)
            {
                DoFight(boardCard, state.Board[2, 0], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[2, 2], CardDirection.Right, CardDirection.Left);
            }
            else if (x == 2 && y == 2)
            {
                DoFight(boardCard, state.Board[2, 1], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[1, 2], CardDirection.Top, CardDirection.Bottom);
            }

            UpdateScore(gameId);
        }

        /// <summary>
        /// Checks to see if there's a player-placed card in every position on the board.
        /// If there is, the game is presumed to be finished.
        /// </summary>
        /// <param name="gameId">The game state Id</param>
        /// <returns>true if the game is finished, false otherwise</returns>
        private static bool CheckForEndCondition(string gameId)
        {
            var state = GameStates[gameId];

            // Check every position in the board for a non-invalid card.
            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    if (state.Board[x, y].CardType == CardType.Invalid ||
                        state.Board[x, y].CardType == CardType.FaceDown)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Ends the game. Player stats are recorded and all players are returned to their last saved position.
        /// </summary>
        /// <param name="gameId">The game Id</param>
        /// <param name="endedPrematurely">If true, a player disconnected or otherwise left the arena. No rewards will be given to either player in this situation.</param>
        private static void EndGame(string gameId, bool endedPrematurely)
        {
            void GrantRewards(uint player, bool? isWinner, bool opponentWasPlayer)
            {
                // No need to reward NPCs or DMs.
                if (!GetIsPC(player) || GetIsDM(player)) return;
                var playerId = GetObjectUUID(player);
                var dbTripleTriadPlayer = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();

                // Won
                if (isWinner == true)
                {
                    if (opponentWasPlayer)
                    {
                        dbTripleTriadPlayer.WinsAgainstPlayers++;
                    }
                    else
                    {
                        dbTripleTriadPlayer.WinsAgainstNPCs++;
                    }

                    dbTripleTriadPlayer.TriadPoints += 2;
                    SendMessageToPC(player, $"Triad Points: {dbTripleTriadPlayer.TriadPoints} (+2)");
                }
                // Lost
                else if (isWinner == false)
                {
                    if (opponentWasPlayer)
                    {
                        dbTripleTriadPlayer.LossesAgainstPlayers++;
                    }
                    else
                    {
                        dbTripleTriadPlayer.LossesAgainstNPCs++;
                    }

                    dbTripleTriadPlayer.TriadPoints++;
                    SendMessageToPC(player, $"Triad Points: {dbTripleTriadPlayer.TriadPoints} (+1)");
                }
                // Draw
                else
                {
                    if (opponentWasPlayer)
                    {
                        dbTripleTriadPlayer.DrawsAgainstPlayers++;
                    }
                    else
                    {
                        dbTripleTriadPlayer.DrawsAgainstNPCs++;
                    }

                    // No Triad Points granted
                    SendMessageToPC(player, $"Triad Points: {dbTripleTriadPlayer.TriadPoints} (+0)");
                }

                DB.Set(playerId, dbTripleTriadPlayer);
            }

            var state = GameStates[gameId];
            var (player1Points, player2Points) = state.CalculatePoints();
            var player1 = state.Players[CardPlayerType.Player1];
            var player2 = state.Players[CardPlayerType.Player2];

            // Player 1 wins
            if (player1Points > player2Points)
            {
                var message = $"{GetName(player1.Player)} has won the game!";
                SendMessageToAllPlayers(gameId, message);

                if (!endedPrematurely)
                {
                    GrantRewards(player1.Player, true, GetIsPC(player2.Player));
                    GrantRewards(player2.Player, false, GetIsPC(player1.Player));
                }
            }
            // Player 2 wins
            else if (player2Points > player1Points)
            {
                var message = $"{GetName(player2.Player)} has won the game!";
                SendMessageToAllPlayers(gameId, message);

                if (!endedPrematurely)
                {
                    GrantRewards(player1.Player, false, GetIsPC(player2.Player));
                    GrantRewards(player2.Player, true, GetIsPC(player1.Player));
                }
            }
            // Draw
            else
            {
                var message = $"The game ended in a draw!";
                SendMessageToAllPlayers(gameId, message);

                if (!endedPrematurely)
                {
                    GrantRewards(player1.Player, null, GetIsPC(player2.Player));
                    GrantRewards(player2.Player, null, GetIsPC(player1.Player));
                }
            }

            // Mark the game as ending. This flag will be picked up in the heartbeat processor.
            // We don't want to immediately end the game as it will create a jarring experience for the player.
            // So instead, we leave the game active for about 18 seconds before cleaning up.
            state.IsGameEnding = true;
        }

        /// <summary>
        /// Updates the textures of the score placeables to match the game scores.
        /// </summary>
        /// <param name="gameId">The game Id</param>
        private static void UpdateScore(string gameId)
        {
            var state = GameStates[gameId];
            var (player1Score, player2Score) = state.CalculatePoints();
            var player1ScoreTexture = GetPowerTexture(player1Score);
            var player2ScoreTexture = GetPowerTexture(player2Score);

            // Replace Main graphic area
            ReplaceObjectTexture(state.Player1ScorePlaceable, DefaultCardTexture, string.Empty);
            ReplaceObjectTexture(state.Player2ScorePlaceable, DefaultCardTexture, string.Empty);
            ReplaceObjectTexture(state.Player1ScorePlaceable, DefaultCardTexture, player1ScoreTexture);
            ReplaceObjectTexture(state.Player2ScorePlaceable, DefaultCardTexture, player2ScoreTexture);
        }

        /// <summary>
        /// Builds a deck of five cards randomly from a given set of levels.
        /// If the level shows up more than once or falls outside of the range 1-10, it will be ignored.
        /// If no cards can be pulled, an exception will be raised.
        /// </summary>
        /// <param name="level">The level to select from</param>
        /// <param name="levels">Optional, additional levels to pull from.</param>
        /// <returns>A deck of five cards</returns>
        public static CardDeck BuildRandomDeck(int level, params int[] levels)
        {
            var deck = new CardDeck{ Name = "Random Deck" };
            var possibleCards = GetAllCardsAtLevel(level).Keys.ToList();

            foreach (var additionalLevel in levels)
            {
                possibleCards.AddRange(GetAllCardsAtLevel(additionalLevel).Keys.ToList());
            }

            // Distinct, in case the same level showed up more than once.
            possibleCards = possibleCards.Distinct().ToList();

            if (possibleCards.Count < 5)
            {
                throw new Exception("Not enough cards are available. Must be at least 5 cards available to build a deck randomly.");
            }

            // Pick a card randomly from the list for each card in the deck.
            // After selecting, remove the card from the possible list so that it doesn't come in twice.
            var index = Random.Next(possibleCards.Count);
            deck.Card1 = possibleCards[index];
            possibleCards.RemoveAt(index);

            index = Random.Next(possibleCards.Count);
            deck.Card2 = possibleCards[index];
            possibleCards.RemoveAt(index);

            index = Random.Next(possibleCards.Count);
            deck.Card3 = possibleCards[index];
            possibleCards.RemoveAt(index);

            index = Random.Next(possibleCards.Count);
            deck.Card4 = possibleCards[index];
            possibleCards.RemoveAt(index);

            index = Random.Next(possibleCards.Count);
            deck.Card5 = possibleCards[index];
            possibleCards.RemoveAt(index);

            return deck;
        }
    }
}
