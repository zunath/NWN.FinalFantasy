using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition
{
    public class SitAction: IAIAction
    {
        public void Action(uint creature, params uint[] targets)
        {
            NWScript.ActionPlayAnimation(Animation.LoopingSitCross);
        }
    }
}
