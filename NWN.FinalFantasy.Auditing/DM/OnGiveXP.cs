using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGiveXP : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var amount = NWNXEvents.OnDMGiveXP_GetAmount();
            var target = NWNXEvents.OnDMGiveXP_GetTarget();
            var targetName = _.GetName(target);

            string log = $"Amount = {amount}, target = {targetName}";

            WriteLog(dm, "Give XP", log);
        }
    }
}