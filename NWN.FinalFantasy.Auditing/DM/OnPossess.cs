using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnPossess : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var target = NWNXEvents.OnDMPossess_GetTarget();
            var targetName = GetName(target);

            if (GetIsObjectValid(target))
            {
                WriteLog(dm, "Possess", $"Target: {targetName}");
            }
            else
            {
                WriteLog(dm, "Unpossess");
            }
        }
    }
}