using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Service;

namespace NWN.FinalFantasy.Feature.LootTableDefinition
{
    public static class TestLootTableDefinition
    {
        [NWNEventHandler("mod_load")]
        public static void RegisterTables()
        {
            var table = new LootTable();
            table.AddItem("nw_waxgr001", 50, 1);
            Loot.AddLootTable("myLoot", table);
        }
    }
}
