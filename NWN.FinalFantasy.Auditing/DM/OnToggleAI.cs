using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnToggleAI : DMTargetListAudit, IScript
    {
        public void Main()
        {
            RunAudit("Toggle AI");
        }
    }
}