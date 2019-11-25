using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnTakeItem : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Take Item");
        }
    }
}