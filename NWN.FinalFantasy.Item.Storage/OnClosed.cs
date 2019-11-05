namespace NWN.FinalFantasy.Item.Storage
{
    public class OnClosed
    {
        public static void Main()
        {
            NWGameObject container = NWGameObject.OBJECT_SELF;
            _.DestroyAllInventoryItems(container);
            _.SetLocked(container, false);
        }
    }
}
