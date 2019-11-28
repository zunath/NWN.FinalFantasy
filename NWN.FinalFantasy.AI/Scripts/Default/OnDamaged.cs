using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnDamaged: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default6", NWGameObject.OBJECT_SELF);
        }
    }
}
