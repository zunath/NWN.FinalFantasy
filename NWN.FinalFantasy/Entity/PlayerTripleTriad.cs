using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using NWN.FinalFantasy.Service.TripleTriadService;

namespace NWN.FinalFantasy.Entity
{
    [MessagePackObject(true)]
    public class PlayerTripleTriad: EntityBase
    {
        public PlayerTripleTriad()
        {
            AvailableCards = new Dictionary<CardType, DateTime>();
            Decks = new Dictionary<int, CardDeck>();
        }

        public override string KeyPrefix => "PlayerTripleTriad";
        public Dictionary<CardType, DateTime> AvailableCards { get; set; }
        public Dictionary<int, CardDeck> Decks { get; set; }

        public uint WinsAgainstPlayers { get; set; }
        public uint LossesAgainstPlayers { get; set; }
        public uint DrawsAgainstPlayers { get; set; }

        public uint WinsAgainstNPCs { get; set; }
        public uint LossesAgainstNPCs{ get; set; }
        public uint DrawsAgainstNPCs { get; set; }

        public uint TriadPoints { get; set; }
        public int LastDeckId { get; set; }
    }
}
