using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGiveItem : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var item = NWNXEvents.OnDMGiveItem_GetItem();
            var itemName = GetName(item);
            var target = NWNXEvents.OnDMGiveItem_GetTarget();
            var targetName = GetName(target);

            string log = $"Item = {itemName}, target = {targetName}";

            WriteLog(dm, "Give Item", log);
        }
    }
}