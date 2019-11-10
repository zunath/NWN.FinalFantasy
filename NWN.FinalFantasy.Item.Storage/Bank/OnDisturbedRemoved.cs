using static NWN._;

namespace NWN.FinalFantasy.Item.Storage.Bank
{
    public class OnDisturbedRemoved: BankStorageBase
    {
        public static void Main()
        {
            var player = GetLastDisturbed();
            var playerID = GetGlobalID(player);
            var key = BuildKey(playerID);
            RemoveItem(key);
        }
    }
}
