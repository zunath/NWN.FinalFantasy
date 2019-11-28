using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnDisturbed: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default8", NWGameObject.OBJECT_SELF);
        }
    }
}
