using System;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.SnippetDefinition
{
    public static class TripleTriadSnippetDefinition
    {
        [Snippet("action-card-game")]
        public static void StartCardGame(uint player, string[] args)
        {
            // todo: Deck selection

            var playerDeck = TripleTriad.BuildRandomDeck(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            var npcDeck = TripleTriad.BuildRandomDeck(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            TripleTriad.StartGame(player, playerDeck, OBJECT_SELF, npcDeck);
        }
    }
}
