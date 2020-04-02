using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    public static class Item
    {
        public static void ReturnItem(uint target, uint item)
        {
            if (GetHasInventory(item))
            {
                var possessor = GetItemPossessor(item);
                AssignCommand(possessor, () =>
                {
                    ActionGiveItem(item, target);
                });
            }
            else
            {
                CopyItem(item, target, true);
                DestroyObject(item);
            }
        }

    }
}
