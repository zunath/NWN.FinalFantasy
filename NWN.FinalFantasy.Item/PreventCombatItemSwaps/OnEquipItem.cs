using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Item.PreventCombatItemSwaps
{
    public class OnEquipItem: PreventCombatItemSwapsBase, IScript
    {
        public void Main()
        {
            Prevent();
        }
    }
}
