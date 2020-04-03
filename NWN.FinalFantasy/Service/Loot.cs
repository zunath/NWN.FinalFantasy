using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWN.FinalFantasy.Service
{
    public static class Loot
    {
        private static readonly Dictionary<string, LootTable> LootTables = new Dictionary<string, LootTable>();

        /// <summary>
        /// Adds a loot table to the registry. If the name is already in use,
        /// an exception will be raised.
        /// </summary>
        /// <param name="name">The name of the loot table</param>
        /// <param name="lootTable">The loot table to register</param>
        public static void AddLootTable(string name, LootTable lootTable)
        {
            if (LootTables.ContainsKey(name))
                throw new Exception($"Loot table '{name}' is already registered. Names must be unique.");

            LootTables[name] = lootTable;
        }

        /// <summary>
        /// Retrieves a loot table by its unique name.
        /// If name is not registered, an exception will be raised.
        /// </summary>
        /// <param name="name">The name of the loot table to retrieve.</param>
        /// <returns>A loot table matching the specified name.</returns>
        public static LootTable GetLootTableByName(string name)
        {
            if(!LootTables.ContainsKey(name))
                throw new Exception($"Loot table '{name}' is not registered. Did you enter the right name?");

            return LootTables[name];
        }
    }

    public class LootTableItem
    {
        public string Resref { get; set; }
        public int MaxQuantity { get; set; }
        public int Weight { get; set; }

        public LootTableItem(string resref, int maxQuantity, int weight)
        {
            Resref = resref;
            MaxQuantity = maxQuantity;
            Weight = weight;
        }
    }

    public class LootTable : List<LootTableItem>
    {
        /// <summary>
        /// Adds an item to this loot table.
        /// </summary>
        /// <param name="resref">The resref of the item</param>
        /// <param name="weightedChance">The weighted chance of the item to drop</param>
        /// <param name="maxQuantity">The max quantity of this item to drop. A random value is selected, not to exceed this value.</param>
        /// <returns>The loot table with the applied changes.</returns>
        public void AddItem(string resref, int weightedChance, int maxQuantity = 1)
        {
            Add(new LootTableItem(resref.ToLower(), maxQuantity, weightedChance));
        }

        /// <summary>
        /// Adds gold to this loot table.
        /// </summary>
        /// <param name="maxAmount">The max amount of gold to drop. A random value is selected, not to exceed this value.</param>
        /// <param name="weightedChance">The weighted chance of the gold to drop</param>
        public void AddGold(int maxAmount, int weightedChance)
        {
            const string GoldResref = "nw_it_gold001";
            Add(new LootTableItem(GoldResref, maxAmount, weightedChance));
        }

        /// <summary>
        /// Retrieves a random item from the loot table.
        /// Throws an exception if there are no items in the loot table.
        /// </summary>
        /// <returns>A random item from the loot table.</returns>
        public LootTableItem GetRandomItem()
        {
            if (Count <= 0)
                throw new Exception("No items are in this loot table.");

            int[] weights = new int[Count];
            for (int x = 0; x < Count; x++)
            {
                weights[x] = this.ElementAt(x).Weight;
            }

            int randomIndex = Random.GetRandomWeightedIndex(weights);
            return this.ElementAt(randomIndex);
        }
    }

}
