namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class PublicStorageBase: StorageBase
    {
        protected static string BuildKey()
        {
            var storageID = GetStorageID();
            return $"PublicStorage:{storageID}";
        }
    }
}
