using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnJumpAllPlayersToPoint : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var position = Vector(
                NWNXEvents.OnDMJumpToPoint_GetX(),
                NWNXEvents.OnDMJumpToPoint_GetY(),
                NWNXEvents.OnDMJumpToPoint_GetZ());
            var area = NWNXEvents.OnDMJumpToPoint_GetArea();
            var areaName = GetName(area);

            var log = $"{areaName} - {position.X}, {position.Y}, {position.Z}";

            WriteLog(dm, "Jump All Players to Point", log);
        }
    }
}