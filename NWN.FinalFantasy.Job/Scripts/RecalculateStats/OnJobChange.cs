using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    internal class OnJobChange: RecalculateStatsBase
    {
        public static void Main()
        {
            var data = Script.GetScriptData<JobChanged>();
            Recalculate(data.Player);
        }
    }
}
