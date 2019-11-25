using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnToggleImmortality : DMTargetListAudit, IScript
    {
        public void Main()
        {
            RunAudit("Toggle Immortality");
        }
    }
}