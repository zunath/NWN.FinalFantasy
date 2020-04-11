using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Feature;

namespace NWN.FinalFantasy.Service.SpawnService
{
    public class SpawnTable
    {
        public string Name { get; set; }
        public int RespawnDelayMinutes { get; set; }
        public List<SpawnObject> Spawns { get; set; }

        public SpawnTable(string name)
        {
            Name = name;
            RespawnDelayMinutes = Spawning.DefaultRespawnMinutes;
            Spawns = new List<SpawnObject>();
        }

        /// <summary>
        /// Retrieves the next spawn resref and object type based on the rules for this specific spawn table.
        /// </summary>
        /// <returns>A tuple cointaining the object type and resref to spawn.</returns>
        public Tuple<ObjectType, string> GetNextSpawnResref()
        {
            var selectedObject = SelectRandomSpawnObject();
            return new Tuple<ObjectType, string>(selectedObject.Type, selectedObject.Resref);
        }

        /// <summary>
        /// Retrieves a random spawn object based on weight.
        /// </summary>
        /// <returns></returns>
        private SpawnObject SelectRandomSpawnObject()
        {
            var weights = Spawns.Select(s => s.Weight).ToArray();
            var index = Random.GetRandomWeightedIndex(weights);
            return Spawns.ElementAt(index);
        }
    }
}
