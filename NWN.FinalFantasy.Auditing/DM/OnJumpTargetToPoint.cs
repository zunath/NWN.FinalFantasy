using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnJumpTargetToPoint : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var position = Vector(
                NWNXEvents.OnDMJumpToPoint_GetX(),
                NWNXEvents.OnDMJumpToPoint_GetY(),
                NWNXEvents.OnDMJumpToPoint_GetZ());
            var area = NWNXEvents.OnDMJumpToPoint_GetArea();
            var targets = NWNXEvents.DMEvents_GetTargetList();
            var areaName = GetName(area);
            var targetNames = string.Empty;

            foreach (var target in targets)
            {
                var targetName = GetName(target);
                targetNames += "(" + targetName + ") ";
            }

            var log = $"{areaName} - {position.X}, {position.Y}, {position.Z} - [{targetNames}]";

            WriteLog(dm, "Jump Target to Point", log);
        }
    }
}