namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class OnAddItem: PublicStorageBase
    {
        public static void Main()
        {
            var key = BuildKey();
            AddItem(key);
        }
    }
}
