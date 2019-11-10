using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    internal class OnLevelUp : RecalculateStatsBase
    {
        public static void Main()
        {
            var data = Script.GetScriptData<LeveledUp>();
            Recalculate(data.Player);
        }
    }
}