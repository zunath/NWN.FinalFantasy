using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnDisableTrap : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var trap = NWNXEvents.OnDMDisableTrap_GetTrap();
            var trapName = GetName(trap);
            WriteLog(dm, "Disable Trap", trapName);
        }
    }
}