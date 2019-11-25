using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnDisappear : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Disappear");
        }
    }
}