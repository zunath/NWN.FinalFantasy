using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AICreatureCommand
    {
        public uint Creature { get; set; }
        public List<uint> CalculatedTargets { get; set; }
        public IAIAction Action { get; set; }

        public AICreatureCommand(uint creature, List<uint> calculatedTargets, IAIAction action)
        {
            Creature = creature;
            CalculatedTargets = calculatedTargets;
            Action = action;
        }
    }
}
