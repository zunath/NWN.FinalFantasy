using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Item.Storage.Bank;

namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class OnOpened : PublicStorageBase, IScript
    {
        public void Main()
        {
            var key = BuildKey();
            OpenStorage(key);
        }
    }
}
