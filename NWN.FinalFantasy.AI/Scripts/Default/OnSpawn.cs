using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnSpawn: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default9", NWGameObject.OBJECT_SELF);
        }
    }
}
