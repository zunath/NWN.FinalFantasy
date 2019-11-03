using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnSpawnObject : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var area = NWNXEvents.OnDMSpawnObject_GetArea();
            var areaName = GetName(area);
            var obj = NWNXEvents.OnDMSpawnObject_GetObject();
            var objName = GetName(obj);
            var type = NWNXEvents.OnDMSpawnObject_GetObjectType();
            var x = NWNXEvents.OnDMSpawnObject_GetPositionX();
            var y = NWNXEvents.OnDMSpawnObject_GetPositionY();
            var z = NWNXEvents.OnDMSpawnObject_GetPositionZ();

            WriteLog(dm, "Spawn Object", $"{areaName} - {objName} - {type} - ({x}, {y}, {z})");
        }
    }
}