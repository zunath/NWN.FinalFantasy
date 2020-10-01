using System.Collections.Generic;
using System.Threading.Tasks;

namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAITargets
    {
        Task<List<uint>> GetTargetsAsync(uint creature);
    }
}
