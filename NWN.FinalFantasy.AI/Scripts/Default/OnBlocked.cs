using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnBlocked: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_defaulte", NWGameObject.OBJECT_SELF);
        }
    }
}
