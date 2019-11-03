using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnDisableTrap : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var trap = NWNXEvents.OnDMDisableTrap_GetTrap();
            var trapName = GetName(trap);
            WriteLog(dm, "Disable Trap", trapName);
        }
    }
}