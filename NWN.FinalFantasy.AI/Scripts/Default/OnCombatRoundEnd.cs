using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnCombatRoundEnd: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_default3", NWGameObject.OBJECT_SELF);
        }
    }
}
