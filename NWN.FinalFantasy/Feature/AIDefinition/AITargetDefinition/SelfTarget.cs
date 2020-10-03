using System.Collections.Generic;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition
{
    public class SelfTarget: IAITargets
    {
        public List<uint> GetTargets(uint creature)
        {
            var list = new List<uint>
            {
                creature
            };

            return list;
        }
    }
}
