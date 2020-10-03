using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Service.AIService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition
{
    public class RandomWalkAction: IAIAction
    {
        public void Action(uint creature, params uint[] targets)
        {
            ActionRandomWalk();
        }
    }
}
