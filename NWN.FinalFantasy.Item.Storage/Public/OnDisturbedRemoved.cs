using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Item.Storage.Public
{
    public class OnDisturbedRemoved: PublicStorageBase, IScript
    {
        public void Main()
        {
            var key = BuildKey();
            RemoveItem(key);
        }
    }
}
