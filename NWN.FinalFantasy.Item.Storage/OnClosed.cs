using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Item.Storage
{
    public class OnClosed: IScript
    {
        public void Main()
        {
            NWGameObject container = NWGameObject.OBJECT_SELF;
            _.DestroyAllInventoryItems(container);
            _.SetLocked(container, false);
        }
    }
}
