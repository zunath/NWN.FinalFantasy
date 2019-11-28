using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.NPCEnmity
{
    public class OnDamaged: IScript
    {
        public void Main()
        {
            NWGameObject self = NWGameObject.OBJECT_SELF;
            NWGameObject damager = _.GetLastDamager(self);
            int enmityAmount = _.GetTotalDamageDealt();
            if (enmityAmount <= 0) enmityAmount = 1;

            Enmity.AdjustEnmity(self, damager, enmityAmount);
        }
    }
}
