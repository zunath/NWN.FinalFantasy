using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Item.Storage.Bank
{
    public class OnOpened : BankStorageBase, IScript
    {
        public void Main()
        {
            var player = GetLastOpenedBy();
            var playerID = GetGlobalID(player);
            var key = BuildKey(playerID);
            OpenStorage(key);
        }
    }
}
