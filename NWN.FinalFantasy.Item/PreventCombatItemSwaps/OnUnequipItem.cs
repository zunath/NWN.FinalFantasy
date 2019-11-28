using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Item.PreventCombatItemSwaps
{
    public class OnUnequipItem: PreventCombatItemSwapsBase, IScript
    {
        public void Main()
        {
            Prevent();
        }
    }
}
