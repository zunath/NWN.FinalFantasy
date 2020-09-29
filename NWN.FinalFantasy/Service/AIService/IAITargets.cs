using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAITargets
    {
        List<uint> GetTargets();
    }
}
