using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGetVariable : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Get Variable");
        }
    }
}