using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    public class OnUnequipItem: RecalculateStatsBase, IScript
    {
        public void Main()
        {
            var player = GetPCItemLastUnequippedBy();
            var item = GetPCItemLastUnequipped();
            SetLocalInt(item, "EXCLUDE_FROM_CALCULATIONS", 1);
            Recalculate(player);
            DeleteLocalInt(item, "EXCLUDE_FROM_CALCULATIONS");
        }
    }
}
