using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts
{
    public class NPCDamaged: IScript
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
