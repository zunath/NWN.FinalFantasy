using System;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public class CreatureAI
    {
        /// <summary>
        /// This is the primary entry point for creature AI.
        /// </summary>
        [NWNEventHandler("crea_roundend")]
        public static void OnCombatRoundEnd()
        {
            var self = OBJECT_SELF;

            // Petrified - do nothing else.
            if (GetHasEffect(EffectTypeScript.Petrify, self)) return;

            // If currently randomly walking, clear all actions.
            if (GetCurrentAction(self) == ActionType.RandomWalk)
            {
                ClearAllActions();
            }

            // Attempt to target the highest enmity creature.
            // If no target can be determined, exit early.
            var target = GetTarget();
            if (target == OBJECT_INVALID) return;

            // Switch targets if necessary.
            if (GetIsInCombat(self))
            {
                if (target != GetAttackTarget(self))
                {
                    AssignCommand(self, () =>
                    {
                        ClearAllActions();
                        ActionAttack(target);
                    });
                }
                
                // Perk ability usage
                var (feat, featTarget) = DeterminePerkAbility(self, target);
                if (feat != Feat.Invalid && GetIsObjectValid(featTarget))
                {
                    AssignCommand(self, () => ActionUseFeat(feat, featTarget));
                }
            }
        }

        /// <summary>
        /// Returns the creature with the highest enmity on this enemy's enmity table.
        /// If no target can be determined, OBJECT_INVALID will be returned.
        /// </summary>
        /// <returns>The creature with the highest enmity, or OBJECT_INVALID if it cannot be determined.</returns>
        private static uint GetTarget()
        {
            var self = OBJECT_SELF;
            var enmityTable = Enmity.GetEnmityTable(self);
            if (enmityTable.Count <= 0) return OBJECT_INVALID;

            var highest = enmityTable.OrderByDescending(o => o.Value).First();
            return highest.Key;
        }

        /// <summary>
        /// Returns whether a creature has an effect.
        /// </summary>
        /// <param name="effectType">The type of effect to look for.</param>
        /// <param name="creature">The creature to check</param>
        /// <returns>true if creature has the effect, false otherwise</returns>
        private static bool GetHasEffect(EffectTypeScript effectType, uint creature)
        {
            var effect = GetFirstEffect(creature);
            while (GetIsEffectValid(effect))
            {
                if (GetEffectType(effect) == effectType)
                {
                    return true;
                }
                effect = GetNextEffect(creature);
            }

            return false;
        }

        /// <summary>
        /// Checks whether a creature can use a specific feat.
        /// Verifies whether a creature has the feat, meets the condition, and can use the ability.
        /// </summary>
        /// <param name="creature">The creature to check</param>
        /// <param name="target">The target of the feat</param>
        /// <param name="feat">The feat to check</param>
        /// <param name="perkType">The type of perk to check</param>
        /// <param name="condition">The custom condition to check</param>
        /// <returns>true if feat can be used, false otherwise</returns>
        private static bool CheckIfCanUseFeat(uint creature, uint target, Feat feat, PerkType perkType, Func<bool> condition = null)
        {
            if (!GetHasFeat(feat, creature)) return false;
            if (condition != null && !condition()) return false;

            var effectiveLevel = Perk.GetEffectivePerkLevel(creature, perkType);
            return Ability.CanUseAbility(creature, target, feat, effectiveLevel);
        }

        /// <summary>
        /// When a creature spawns, store their STM and MP as local variables.
        /// </summary>
        [NWNEventHandler("crea_spawn")]
        public static void LoadCreatureStats()
        {
            var self = OBJECT_SELF;
            var mpStats = GetAbilityModifier(AbilityType.Intelligence, self) +
                        GetAbilityModifier(AbilityType.Wisdom, self);
            var stmStats = GetAbilityModifier(AbilityType.Constitution, self);
            var mp = mpStats * 2;
            var stm = stmStats * 2;

            SetLocalInt(self, "MAX_MP", mp);
            SetLocalInt(self, "MAX_STAMINA", stm);
            SetLocalInt(self, "MP", mp);
            SetLocalInt(self, "STAMINA", stm);
        }

        /// <summary>
        /// When a creature's heartbeat fires, restore their STM and MP.
        /// </summary>
        [NWNEventHandler("crea_heartbeat")]
        public static void RestoreCreatureStats()
        {
            var self = OBJECT_SELF;
            var maxMP = GetLocalInt(self, "MAX_MP");
            var maxSTM = GetLocalInt(self, "MAX_STAMINA");
            var mp = GetLocalInt(self, "MP") + 1;
            var stm = GetLocalInt(self, "STAMINA") + 1;

            if (mp > maxMP)
                mp = maxMP;
            if (stm > maxSTM)
                stm = maxSTM;

            SetLocalInt(self, "MP", mp);
            SetLocalInt(self, "STAMINA", stm);
        }

        /// <summary>
        /// Determines which perk ability to use.
        /// </summary>
        /// <param name="self">The creature</param>
        /// <param name="target">The creature's target</param>
        /// <returns>A feat and target</returns>
        private static (Feat, uint) DeterminePerkAbility(uint self, uint target)
        {
            var currentHP = GetCurrentHitPoints(self);
            var maxHP = GetMaxHitPoints(self);
            var hpPercentage = ((float)currentHP / (float)maxHP) * 100;

            // 1-hour Defensives
            if (CheckIfCanUseFeat(self, self, Feat.Benediction, PerkType.Benediction, () => hpPercentage <= 20f))
            {
                return (Feat.Benediction, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Invincible, PerkType.Invincible, () => hpPercentage <= 30f))
            {
                return (Feat.Invincible, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.PerfectDodge, PerkType.PerfectDodge, () => hpPercentage <= 65f))
            {
                return (Feat.PerfectDodge, self);
            }

            // 1-hour Offensives
            if (CheckIfCanUseFeat(self, self, Feat.HundredFists, PerkType.HundredFists, () => hpPercentage <= 90f))
            {
                return (Feat.HundredFists, self);
            }

            // HP Restoration
            if (CheckIfCanUseFeat(self, self, Feat.Cure3, PerkType.Cure, () => hpPercentage <= 50f))
            {
                return (Feat.Cure3, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Cure2, PerkType.Cure, () => hpPercentage <= 75f))
            {
                return (Feat.Cure2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Cure1, PerkType.Cure, () => hpPercentage <= 80f))
            {
                return (Feat.Cure1, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.InnerHealing5, PerkType.InnerHealing, () => hpPercentage <= 50f))
            {
                return (Feat.InnerHealing5, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.InnerHealing4, PerkType.InnerHealing, () => hpPercentage <= 60f))
            {
                return (Feat.InnerHealing4, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.InnerHealing3, PerkType.InnerHealing, () => hpPercentage <= 70f))
            {
                return (Feat.InnerHealing3, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.InnerHealing2, PerkType.InnerHealing, () => hpPercentage <= 80f))
            {
                return (Feat.InnerHealing2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.InnerHealing1, PerkType.InnerHealing, () => hpPercentage <= 90f))
            {
                return (Feat.InnerHealing1, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Regen2, PerkType.Regen, () => hpPercentage <= 85f))
            {
                return (Feat.Regen2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Regen1, PerkType.Regen, () => hpPercentage <= 90f))
            {
                return (Feat.Regen1, self);
            }

            // Status Restoration
            if (CheckIfCanUseFeat(self, self, Feat.Poisona, PerkType.Poisona, () =>
            {
                return StatusEffect.HasStatusEffect(self, StatusEffectType.Poison3) ||
                       StatusEffect.HasStatusEffect(self, StatusEffectType.Poison2) ||
                       StatusEffect.HasStatusEffect(self, StatusEffectType.Poison1);
            }))
            {
                return (Feat.Poisona, self);
            }

            // Buffs
            if (CheckIfCanUseFeat(self, self, Feat.Defender3, PerkType.Defender, () => !StatusEffect.HasStatusEffect(self, StatusEffectType.Defender3)))
            {
                return (Feat.Defender3, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Defender2, PerkType.Defender, () => !StatusEffect.HasStatusEffect(self, StatusEffectType.Defender2)))
            {
                return (Feat.Defender2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Defender1, PerkType.Defender, () => !StatusEffect.HasStatusEffect(self, StatusEffectType.Defender1)))
            {
                return (Feat.Defender1, self);
            }

            if (CheckIfCanUseFeat(self, self, Feat.Ironclad3, PerkType.Ironclad, () => !StatusEffect.HasStatusEffect(self, StatusEffectType.Ironclad3)))
            {
                return (Feat.Ironclad3, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Ironclad2, PerkType.Ironclad, () => !StatusEffect.HasStatusEffect(self, StatusEffectType.Ironclad2)))
            {
                return (Feat.Ironclad2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Ironclad1, PerkType.Ironclad, () => !StatusEffect.HasStatusEffect(self, StatusEffectType.Ironclad1)))
            {
                return (Feat.Ironclad1, self);
            }

            if (CheckIfCanUseFeat(self, self, Feat.Flee2, PerkType.Flee, () => hpPercentage <= 20f))
            {
                return (Feat.Flee2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Flee1, PerkType.Flee, () => hpPercentage <= 20f))
            {
                return (Feat.Flee1, self);
            }

            if (CheckIfCanUseFeat(self, self, Feat.BlazeSpikes2, PerkType.BlazeSpikes))
            {
                return (Feat.BlazeSpikes2, self);
            }
            if (CheckIfCanUseFeat(self, self, Feat.BlazeSpikes1, PerkType.BlazeSpikes))
            {
                return (Feat.BlazeSpikes1, self);
            }



            // Debuffs
            if (CheckIfCanUseFeat(self, target, Feat.Flash2, PerkType.Flash))
            {
                return (Feat.Flash2, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Flash1, PerkType.Flash))
            {
                return (Feat.Flash1, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Fire3, PerkType.Fire))
            {
                return (Feat.Fire3, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Fire2, PerkType.Fire))
            {
                return (Feat.Fire2, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Fire1, PerkType.Fire))
            {
                return (Feat.Fire1, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Blizzard3, PerkType.Blizzard))
            {
                return (Feat.Blizzard3, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Blizzard2, PerkType.Blizzard))
            {
                return (Feat.Blizzard2, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Blizzard1, PerkType.Blizzard))
            {
                return (Feat.Blizzard1, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Thunder3, PerkType.Thunder))
            {
                return (Feat.Thunder3, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Thunder2, PerkType.Thunder))
            {
                return (Feat.Thunder2, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Thunder1, PerkType.Thunder))
            {
                return (Feat.Thunder1, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Stone3, PerkType.Stone))
            {
                return (Feat.Stone3, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Stone2, PerkType.Stone))
            {
                return (Feat.Stone2, target);
            }
            if (CheckIfCanUseFeat(self, target, Feat.Stone1, PerkType.Stone))
            {
                return (Feat.Stone1, target);
            }

            return (Feat.Invalid, OBJECT_INVALID);
        }

    }
}
