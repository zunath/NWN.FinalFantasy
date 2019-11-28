using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnUserDefined: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_defaultd", NWGameObject.OBJECT_SELF);
        }
    }
}
