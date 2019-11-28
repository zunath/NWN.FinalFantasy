using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnDeath: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default7", NWGameObject.OBJECT_SELF);
        }
    }
}
