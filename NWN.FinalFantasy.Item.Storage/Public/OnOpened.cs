using NWN.FinalFantasy.Item.Storage.Bank;

namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class OnOpened : PublicStorageBase
    {
        public static void Main()
        {
            var key = BuildKey();
            OpenStorage(key);
        }
    }
}
