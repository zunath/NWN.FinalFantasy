using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.NPCEnmity
{
    public class OnDeath: IScript
    {
        public void Main()
        {
            var npc = NWGameObject.OBJECT_SELF;
            if (!_.GetIsNPC(npc)) return;

            var npcID = _.GetGlobalID(npc);
            Enmity.ClearEnmityTable(npcID);
        }
    }
}
