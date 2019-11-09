using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Event
{
    internal class PreventNonClassItems
    {
        public static void Main()
        {
            var player = NWGameObject.OBJECT_SELF;
            if (GetIsDungeonMaster(player)) return;

            var item = NWNXEvents.OnEquipItem_GetItem();
            var itemType = GetBaseItemType(item);
            var slot = NWNXEvents.OnEquipItem_GetInventorySlot();
            var @class = GetClassByPosition(ClassPosition.First, player);
            var job = JobRegistry.Get(@class);
            

        }
    }
}
