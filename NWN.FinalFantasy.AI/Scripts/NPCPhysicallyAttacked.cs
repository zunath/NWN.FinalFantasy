using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.AI.Scripts
{
    public class NPCPhysicallyAttacked: IScript
    {
        public void Main()
        {
            NWGameObject npc = NWGameObject.OBJECT_SELF;
            NWGameObject attacker = GetLastAttacker(npc);

            Enmity.AdjustEnmity(npc, attacker, 1);
        }
    }
}
