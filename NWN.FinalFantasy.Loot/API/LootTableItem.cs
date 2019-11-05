namespace NWN.FinalFantasy.Loot.API
{
    public class LootTableItem: ILootTableItem
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
}
