using static NWN._;

namespace NWN.FinalFantasy.Item.Storage.Bank
{
    public class OnOpened : BankStorageBase
    {
        public static void Main()
        {
            var player = GetLastOpenedBy();
            var playerID = GetGlobalID(player);
            var key = BuildKey(playerID);
            OpenStorage(key);
        }
    }
}
