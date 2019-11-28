using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnHeal : DMTargetListAudit, IScript
    {
        public void Main()
        {
            RunAudit("Heal");
        }
    }
}