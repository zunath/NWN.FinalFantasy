using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public abstract class DMTargetListAudit: DMAudit
    {
        protected static void RunAudit(string @event)
        {
            var dm = NWGameObject.OBJECT_SELF;
            var targets = NWNXEvents.DMEvents_GetTargetList();
            var details = "Target List: ";

            foreach (var target in targets)
            {
                var name = GetName(target);
                details += "(" + name + ") ";
            }

            WriteLog(dm, @event, details);
        }
    }
}
