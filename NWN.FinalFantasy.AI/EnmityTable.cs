using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.AI
{
    public class EnmityTable: Dictionary<Guid, EnmityTarget>
    {
        public NWGameObject NPC { get; set; }

        public EnmityTable(NWGameObject npc)
        {
            NPC = npc;
        }
    }
}
