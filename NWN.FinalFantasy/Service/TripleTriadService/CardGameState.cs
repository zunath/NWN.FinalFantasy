using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public class CardGameState
    {
        public uint ArenaArea { get; set; }

        public bool HasInitialized { get; set; }
        public Dictionary<CardPlayerType, CardPlayer> Players { get; set; }

        public CardBoardPosition[,] Board { get; set; }
        public CardPlayerType CurrentPlayerTurn { get; set; }
        public CardRuleType Rules { get; set; }

        public int DisconnectionCheckCounter { get; set; }

        public uint Player1ScorePlaceable { get; set; }
        public uint Player2ScorePlaceable { get; set; }

        public int GameEndingTicks { get; set; }
        public bool IsGameEnding { get; set; }
        public bool IsGameCleaningUp { get; set; }

        public bool IsPlayingSelf => Players[CardPlayerType.Player1].Player == Players[CardPlayerType.Player2].Player;

        /// <summary>
        /// Calculates the points for each player.
        /// Cards in hands count for 1 point for their owner.
        /// Cards on the board count for 1 point for whoever has claimed them.
        /// </summary>
        /// <returns>The number of points for each player.</returns>
        public (int, int) CalculatePoints()
        {
            // Cards in hands count for one point each.
            var player1Points = Players[CardPlayerType.Player1].Hand.Count;
            var player2Points = Players[CardPlayerType.Player2].Hand.Count;

            // Iterate over the board and count up the number of cards owned by this player.
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    // Make sure there's a card in this position.
                    if (Board[x, y] != null)
                    {
                        var card = Board[x, y];

                        if (card.CurrentOwner == CardPlayerType.Player1)
                        {
                            player1Points++;
                        }
                        else if (card.CurrentOwner == CardPlayerType.Player2)
                        {
                            player2Points++;
                        }
                    }
                }
            }

            return (player1Points, player2Points);
        }

        public CardGameState(uint arenaArea, uint player1, uint player2)
        {
            ArenaArea = arenaArea;
            Players = new Dictionary<CardPlayerType, CardPlayer>
            {
                [CardPlayerType.Player1] = new CardPlayer(player1),
                [CardPlayerType.Player2] = new CardPlayer(player2)
            };

            Board = new CardBoardPosition[3,3];
            Board[0, 0] = new CardBoardPosition();
            Board[0, 1] = new CardBoardPosition();
            Board[0, 2] = new CardBoardPosition();

            Board[1, 0] = new CardBoardPosition();
            Board[1, 1] = new CardBoardPosition();
            Board[1, 2] = new CardBoardPosition();

            Board[2, 0] = new CardBoardPosition();
            Board[2, 1] = new CardBoardPosition();
            Board[2, 2] = new CardBoardPosition();
        }
    }
}
