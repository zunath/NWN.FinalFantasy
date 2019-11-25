using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class OnAddItem: PublicStorageBase, IScript
    {
        public void Main()
        {
            var key = BuildKey();
            AddItem(key);
        }
    }
}
