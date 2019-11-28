using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnForceRest : DMTargetListAudit, IScript
    {
        public void Main()
        {
            RunAudit("Force Rest");
        }
    }
}