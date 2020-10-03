﻿using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Feature.AIDefinition.AIData;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition
{
    public class FirstPlayerTarget: IAITargets
    {
        public List<uint> GetTargets(uint creature)
        {
            var creatureData = AICreatureData.GetCreature(creature);
            var players = AIPlayerData.GetPlayersInArea(creatureData.Area).ToList();
            var playerList = new List<uint>();

            if(players.Any())
                playerList.Add(players.ElementAt(0).Player);

            return playerList;
        }
    }
}