namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class OnDisturbedRemoved: PublicStorageBase
    {
        public static void Main()
        {
            var key = BuildKey();
            RemoveItem(key);
        }
    }
}
