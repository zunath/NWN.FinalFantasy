using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class WeaponMPRegenAbilityDefinition
    {
        /// <summary>
        /// When an item's OnHit script fires, run the Battlemage ability.
        /// </summary>
        [NWNEventHandler("item_on_hit")]
        public static void ApplyBattlemageAbility()
        {
            var attacker = OBJECT_SELF;
            var item = GetSpellCastItem();
            var target = GetSpellTargetObject();

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


        /// <summary>
        /// When an item's OnHit script fires, run the Combat Mage ability.
        /// </summary>
        [NWNEventHandler("item_on_hit")]
        public static void ApplyCombatMageAbility()
        {
            var attacker = OBJECT_SELF;
            var item = GetSpellCastItem();
            var target = GetSpellTargetObject();

            if (!GetIsPC(attacker)) return;
            if (GetObjectType(target) != ObjectType.Creature) return;

            var baseItemType = GetBaseItemType(item);
            if (baseItemType != BaseItem.LightMace) return;

            var perkLevel = Perk.GetEffectivePerkLevel(attacker, PerkType.CombatMage);
            if (perkLevel <= 0) return;

            var playerId = GetObjectUUID(attacker);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.RestoreMP(attacker, dbPlayer, perkLevel);
            DB.Set(playerId, dbPlayer);
        }
    }
}
