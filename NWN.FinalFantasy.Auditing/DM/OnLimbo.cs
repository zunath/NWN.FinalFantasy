using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnLimbo : DMTargetListAudit, IScript
    {
        public void Main()
        {
            RunAudit("Limbo");
        }
    }
}