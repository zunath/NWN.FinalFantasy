using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnHeartbeat: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default1", NWGameObject.OBJECT_SELF);
        }
    }
}
