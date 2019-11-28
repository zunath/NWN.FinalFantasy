using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.AI.Scripts.Default
{
    public class OnSpellCastAt: IScript
    {
        public void Main()
        {
            _.ExecuteScript("nw_c2_defaultb", NWGameObject.OBJECT_SELF);
        }
    }
}
