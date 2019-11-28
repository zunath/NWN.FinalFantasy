using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnRested: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_defaulta", NWGameObject.OBJECT_SELF);
        }
    }
}
