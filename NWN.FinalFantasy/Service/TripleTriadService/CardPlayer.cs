using System;
using System.Collections.Generic;
using System.Text;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public class CardPlayer
    {
        public CardPlayer(uint player)
        {
            Player = player;
            Hand = new Dictionary<int, CardHand>
            {
                {1, new CardHand()},
                {2, new CardHand()},
                {3, new CardHand()},
                {4, new CardHand()},
                {5, new CardHand()},
            };
            CardSelection = new CardSelection();
        }
        public uint Player { get; set; }
        public Dictionary<int, CardHand> Hand { get; set; }
        public CardSelection CardSelection { get; set; }
        public int DecisionCounter { get; set; }
    }
}
