namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    internal class OnEquipItem: RecalculateStatsBase
    {
        public static void Main()
        {
            var player = _.GetPCItemLastEquippedBy();
            Recalculate(player);
        }
    }
}
