namespace NWN.FinalFantasy.Service.LootService
{
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
}
