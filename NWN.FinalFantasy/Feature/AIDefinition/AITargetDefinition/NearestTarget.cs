using System.Collections.Generic;
using System.Threading;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition
{
    public class NearestTarget: IAITargets
    {
        public List<uint> GetTargets()
        {
            Thread.Sleep(200);
            return new List<uint>(AI.Players);
        }
    }
}
