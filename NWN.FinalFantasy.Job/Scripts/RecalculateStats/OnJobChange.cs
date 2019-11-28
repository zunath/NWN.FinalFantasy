using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    public class OnJobChange: RecalculateStatsBase, IScript
    {
        public void Main()
        {
            var data = Script.GetScriptData<JobChanged>();
            Recalculate(data.Player);
        }
    }
}
