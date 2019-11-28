using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    public class OnEquipItem: RecalculateStatsBase, IScript
    {
        public void Main()
        {
            var player = _.GetPCItemLastEquippedBy();
            Recalculate(player);
        }
    }
}
