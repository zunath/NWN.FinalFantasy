using System.Linq;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    public class CheckDualWield: IScript
    {
        public void Main()
        {
            var player = NWGameObject.OBJECT_SELF;
            var slot = NWNXEvents.OnEquipItem_GetInventorySlot();

            // If item being equipped isn't going to the left hand, exit early.
            if (slot != InventorySlot.LeftHand) return;

            // If the main hand item is invalid, exit early.
            var main = GetItemInSlot(InventorySlot.RightHand, player);
            if (!GetIsObjectValid(main)) return;

            // If either one of the items is a non-weapon, exit early.
            var item = NWNXEvents.OnEquipItem_GetItem();
            var mainType = GetBaseItemType(main);
            var offType = GetBaseItemType(item);
            if (!_weaponTypes.Contains(mainType) ||
                !_weaponTypes.Contains(offType))
                return;

            // Check for the Dual Wield feat
            if(!GetHasFeat(Feat.DualWield, player))
            {
                NWNXEvents.SkipEvent();
                SendMessageToPC(player, "You must have the Dual Wield ability to equip two weapons.");
                return;
            }
        }

        private static readonly BaseItemType[] _weaponTypes =
        {
            BaseItemType.Shortsword,
            BaseItemType.BastardSword,
            BaseItemType.Longsword,
            BaseItemType.Battleaxe,
            BaseItemType.LightFlail,
            BaseItemType.Lightmace,
            BaseItemType.Dagger,
            BaseItemType.Club,
            BaseItemType.LightHammer,
            BaseItemType.HandAxe,
            BaseItemType.Kama,
            BaseItemType.Katana,
            BaseItemType.Kukri,
            BaseItemType.Morningstar,
            BaseItemType.Rapier,
            BaseItemType.Scimitar,
            BaseItemType.ShortSpear,
            BaseItemType.Sickle,
            BaseItemType.Whip
        };
    }
}
