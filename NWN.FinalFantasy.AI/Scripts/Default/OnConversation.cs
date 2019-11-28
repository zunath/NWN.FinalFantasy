using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnConversation: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default4", NWGameObject.OBJECT_SELF);
        }
    }
}
