using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGiveGold : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var amount = NWNXEvents.OnDMGiveGold_GetAmount();
            var target = NWNXEvents.OnDMGiveGold_GetTarget();
            var targetName = _.GetName(target);

            string log = $"Amount = {amount}, target = {targetName}";

            WriteLog(dm, "Give Gold", log);
        }
    }
}