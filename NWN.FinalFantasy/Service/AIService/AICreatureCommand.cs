using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AICreatureCommand
    {
        public uint Creature { get; set; }
        public IEnumerable<uint> CalculatedTargets { get; set; }
        public IAIAction Action { get; set; }

        public AICreatureCommand(uint creature, IEnumerable<uint> calculatedTargets, IAIAction action)
        {
            Creature = creature;
            CalculatedTargets = calculatedTargets;
            Action = action;
        }
    }
}
