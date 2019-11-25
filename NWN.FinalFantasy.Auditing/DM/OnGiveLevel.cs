using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGiveLevel : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var amount = NWNXEvents.OnDMGiveLevels_GetAmount();
            var target = NWNXEvents.OnDMGiveLevels_GetTarget();
            var targetName = _.GetName(target);

            string log = $"Amount = {amount}, target = {targetName}";

            WriteLog(dm, "Give Level", log);
        }
    }
}