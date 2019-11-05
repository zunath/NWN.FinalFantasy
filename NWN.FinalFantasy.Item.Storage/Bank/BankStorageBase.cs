using System;

namespace NWN.FinalFantasy.Item.Storage.Bank
{
    public class BankStorageBase: StorageBase
    {
        protected static string BuildKey(Guid playerID)
        {
            var storageID = GetStorageID();
            string key = $"Bank:{storageID}:{playerID}";
            return key;
        }
    }
}
