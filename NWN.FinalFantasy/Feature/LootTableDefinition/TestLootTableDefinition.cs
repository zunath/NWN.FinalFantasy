using System.Collections.Generic;
using NWN.FinalFantasy.Service.LootService;

namespace NWN.FinalFantasy.Feature.LootTableDefinition
{
    public class TestLootTableDefinition: ILootTableDefinition
    {
        public Dictionary<string, LootTable> BuildLootTables()
        {
            var builder = new LootTableBuilder()
                .Create("myLoot")
                .AddItem("nw_waxgr001", 50);

            return builder.Build();
        }
    }
}
