using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Item
{
    internal class AttachSystemPropertiesToItem
    {
        public static void Main()
        {
            var player = NWGameObject.OBJECT_SELF;
            if (!GetIsPlayer(player)) return;

            var item = NWNXEvents.OnEquipItem_GetItem();

            // Didn't find the item property. Add it now.
            SafeAddItemProperty(item, ItemPropertyOnHitCastSpell(IPConst.Onhit_CastSpell_FFOSystem, 40), 0.0f, AddItemPropertyPolicy.ReplaceExisting, true, true);
        }
    }
}
