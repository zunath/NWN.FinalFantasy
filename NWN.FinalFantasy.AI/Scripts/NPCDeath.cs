using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.AI.Scripts
{
    public class NPCDeath: IScript
    {
        public void Main()
        {
            var npc = NWGameObject.OBJECT_SELF;
            if (!GetIsNPC(npc)) return;

            var npcID = GetGlobalID(npc);
            Enmity.ClearEnmityTable(npcID);
        }
    }
}
