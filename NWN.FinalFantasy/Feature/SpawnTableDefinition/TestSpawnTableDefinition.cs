using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Service.SpawnService;

namespace NWN.FinalFantasy.Feature.SpawnTableDefinition
{
    public class TestSpawnTableDefinition: ISpawnTableDefinition
    {
        public Dictionary<int, SpawnTable> BuildSpawnTables()
        {
            return new Dictionary<int, SpawnTable>
            {
                { 1, new SpawnTable
                {

                } }
            };
        }
    }
}
