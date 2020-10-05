using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition
{
    public class SlowCondition: IAICondition
    {
        private static Random _random = new Random();

        public bool MeetsCondition(uint creature)
        {
            var until = DateTime.UtcNow.AddMilliseconds(_random.Next(100));

            while (DateTime.UtcNow < until)
            {

            }


            return true;
        }
    }
}
