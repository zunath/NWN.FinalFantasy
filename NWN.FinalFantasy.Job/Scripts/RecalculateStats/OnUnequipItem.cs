using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    internal class OnUnequipItem: RecalculateStatsBase
    {
        public static void Main()
        {
            var player = GetPCItemLastUnequippedBy();
            var item = GetPCItemLastUnequipped();
            SetLocalInt(item, "EXCLUDE_FROM_CALCULATIONS", 1);
            Recalculate(player);
            DeleteLocalInt(item, "EXCLUDE_FROM_CALCULATIONS");
        }
    }
}
