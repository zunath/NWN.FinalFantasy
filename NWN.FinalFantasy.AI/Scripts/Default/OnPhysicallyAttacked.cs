using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnPhysicallyAttacked: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default5", NWGameObject.OBJECT_SELF);
        }
    }
}
