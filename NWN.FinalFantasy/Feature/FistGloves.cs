using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public static class FistGloves
    {
        [NWNEventHandler("mod_enter")]
        public static void EquipFistGloveOnEntry()
        {
            var player = GetEnteringObject();
            if (!GetIsPC(player) || GetIsDM(player)) return;

            ForceEquipFistGlove(player);
        }

        [NWNEventHandler("mod_unequip")]
        public static void EquipFistGloveOnUnequip()
        {
            var player = GetPCItemLastUnequippedBy();
            if (!GetIsPC(player) || GetIsDM(player)) return;

            var item = GetPCItemLastUnequipped();
            var type = GetBaseItemType(item);

            if (type != BaseItem.Bracer && type != BaseItem.Gloves) return;
            var resref = GetResRef(item);

            // If fist was unequipped, destroy it.
            if (resref == "fist")
            {
                DestroyObject(item);
            }

            // Remove any other fists in the PC's inventory.
            var inventory = GetFirstItemInInventory(player);
            while(GetIsObjectValid(inventory))
            {
                if (GetResRef(inventory) == "fist")
                {
                    DestroyObject(inventory);
                }

                inventory = GetNextItemInInventory(player);
            }

            // Check in 1 second to see if PC has a glove equipped. If they don't, create a fist glove and equip it.
            ForceEquipFistGlove(player);
        }

        /// <summary>
        /// Checks a player's gloves slot. If it's empty, a fist item will be created and the player will equip it.
        /// </summary>
        /// <param name="player">The player who will equip the fist item.</param>
        private static void ForceEquipFistGlove(uint player)
        {
            DelayCommand(1.0f, () =>
            {
                var gloves = GetItemInSlot(InventorySlot.Arms, player);
                if (!GetIsObjectValid(gloves))
                {
                    AssignCommand(player, () => ClearAllActions());
                    var glove = CreateItemOnObject("fist", player);
                    AssignCommand(player, () => ActionEquipItem(glove, InventorySlot.Arms));
                }
            });
        }
    }
}
