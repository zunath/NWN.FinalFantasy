using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Object = NWN.FinalFantasy.Core.NWNX.Object;
using Player = NWN.FinalFantasy.Entity.Player;

namespace NWN.FinalFantasy.Feature
{
    public static class EquipmentRestrictions
    {
        /// <summary>
        /// When an item's usage is validated, check the custom rules to see if the item can be used by the player.
        /// If not able to be used, item will appear red in inventory.
        /// </summary>
        [NWNEventHandler("item_valid_bef")]
        public static void ValidateItemUse()
        {
            var creature = OBJECT_SELF;
            var item = Object.StringToObject(Events.GetEventData("ITEM_OBJECT_ID"));

            Events.SetEventResult(string.IsNullOrWhiteSpace(CanItemBeUsed(creature, item)) ? "1" : "0");
            Events.SkipEvent();
        }

        /// <summary>
        /// When an item is equipped, check the custom rules to see if the item can be equipped by the player.
        /// If not able to be used, an error message will be sent and item will not be equipped.
        /// </summary>
        [NWNEventHandler("item_eqp_bef")]
        public static void ValidateItemEquip()
        {
            var creature = OBJECT_SELF;
            var item = Object.StringToObject(Events.GetEventData("ITEM"));

            var error = CanItemBeUsed(creature, item);
            if (string.IsNullOrWhiteSpace(error)) return;

            SendMessageToPC(creature, ColorToken.Red(error));
            Events.SkipEvent();
        }

        /// <summary>
        /// Checks if an item can be used by a creature. Non-PCs and DMs automatically can wear all items.
        /// If a player is missing a required perk, an error message will be returned.
        /// </summary>
        /// <param name="creature">The creature to check.</param>
        /// <param name="item">The item to check.</param>
        /// <returns>An empty string if successful or an error message if failed</returns>
        private static string CanItemBeUsed(uint creature, uint item)
        {
            if (!GetIsPC(creature) || GetIsDM(creature)) return string.Empty;

            var playerId = GetObjectUUID(creature);
            var dbPlayer = DB.Get<Player>(playerId);

            // Check for required perk levels.
            for (var ip = GetFirstItemProperty(item); GetIsItemPropertyValid(ip); ip = GetNextItemProperty(item))
            {
                var type = GetItemPropertyType(ip);
                if (type != ItemPropertyType.UseLimitationPerk) continue;

                var perkType = (PerkType)GetItemPropertySubType(ip);
                var requiredLevel = GetItemPropertyCostTableValue(ip);
                var perkLevel = dbPlayer.Perks.ContainsKey(perkType) ? dbPlayer.Perks[perkType] : 0;

                if (perkLevel < requiredLevel)
                {
                    var perkName = Perk.GetPerkDetails(perkType).Name;
                    return $"This item requires '{perkName}' level {requiredLevel} to use.";
                }
            }

            return string.Empty;
        }
    }
}
