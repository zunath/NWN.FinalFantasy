using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public static class WeaponRegenPerks
    {
        /// <summary>
        /// When an item's OnHit script fires, run item-specific perks related to regen on-hit.
        /// </summary>
        [NWNEventHandler("item_on_hit")]
        public static void ActivateWeaponRegenPerk()
        {
            var attacker = OBJECT_SELF;
            var item = GetSpellCastItem();
            var target = GetSpellTargetObject();

            Battlemage(attacker, target, item);
        }

        /// <summary>
        /// Restore MP based on the attacker's perk level in Battlemage.
        /// </summary>
        /// <param name="attacker">The attacker</param>
        /// <param name="target">The target of the attack</param>
        /// <param name="item">The item being used for the attack.</param>
        private static void Battlemage(uint attacker, uint target, uint item)
        {
            if (!GetIsPC(attacker)) return;
            if (GetObjectType(target) != ObjectType.Creature) return;

            var baseItemType = GetBaseItemType(item);
            if (baseItemType != BaseItem.QuarterStaff) return;

            var perkLevel = Perk.GetEffectivePerkLevel(attacker, PerkType.Battlemage);
            if (perkLevel <= 0) return;

            var playerId = GetObjectUUID(attacker);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.RestoreMP(attacker, dbPlayer, perkLevel);
            DB.Set(playerId, dbPlayer);
        }
    }
}
