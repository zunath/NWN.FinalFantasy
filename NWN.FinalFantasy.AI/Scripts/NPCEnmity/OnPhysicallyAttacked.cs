using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.NPCEnmity
{
    public class OnPhysicallyAttacked: IScript
    {
        public void Main()
        {
            NWGameObject npc = NWGameObject.OBJECT_SELF;
            NWGameObject attacker = _.GetLastAttacker(npc);

            Enmity.AdjustEnmity(npc, attacker, 1);
        }
    }
}
