using System;
using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Item.Storage.Bank
{
    public class OnDisturbedRemoved: BankStorageBase, IScript
    {
        public void Main()
        {
            var player = GetLastDisturbed();

            Console.WriteLine("player = " + GetName(player));

            var playerID = GetGlobalID(player);
            var key = BuildKey(playerID);
            RemoveItem(key);
        }
    }
}
