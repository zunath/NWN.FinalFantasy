using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnKill : DMTargetListAudit, IScript
    {
        public void Main()
        {
            RunAudit("Kill");
        }
    }
}