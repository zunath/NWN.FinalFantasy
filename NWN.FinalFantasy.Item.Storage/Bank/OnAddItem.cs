using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Item.Storage.Bank
{
    public class OnAddItem: BankStorageBase, IScript
    {
        public void Main()
        {
            var player = NWNXEvents.OnInventoryAddItem_GetPlayer();
            var playerID = GetGlobalID(player);
            var key = BuildKey(playerID);
            AddItem(key);
        }
    }
}
