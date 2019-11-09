using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Job.Event
{
    internal class OnEquipItem
    {
        public static void Main()
        {
            var player = NWGameObject.OBJECT_SELF;
            if (GetIsDungeonMaster(player)) return;

            var item = NWNXEvents.OnEquipItem_GetItem();
            var slot = NWNXEvents.OnEquipItem_GetInventorySlot();



        }
    }
}
