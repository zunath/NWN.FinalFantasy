using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition
{
    public class SlowCondition: IAICondition
    {
        public bool MeetsCondition(uint creature)
        {
            //Thread.Sleep(50);

            return true;
        }
    }
}
