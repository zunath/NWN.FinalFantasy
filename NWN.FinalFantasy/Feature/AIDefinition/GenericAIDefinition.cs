using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AIDefinition
{
    public static class GenericAIDefinition
    {
        /// <summary>
        /// Determines which perk ability to use.
        /// </summary>
        /// <param name="self">The creature</param>
        /// <param name="target">The creature's target</param>
        /// <param name="allies">Allies associated with this creature. Should also include this creature.</param>
        /// <returns>A feat and target</returns>
        public static (Feat, uint) DeterminePerkAbility(uint self, uint target, HashSet<uint> allies)
        {
            static float CalculateAverageHP(uint creature)
            {
                var currentHP = GetCurrentHitPoints(creature);
                var maxHP = GetMaxHitPoints(creature);
                return ((float)currentHP / (float)maxHP) * 100;
            }

            var hpPercentage = CalculateAverageHP(self);

            var lowestHPAlly = allies.OrderBy(CalculateAverageHP).First();
            var allyHPPercentage = CalculateAverageHP(lowestHPAlly);
            var selfRace = GetRacialType(self);
            var lowestHPAllyRace = GetRacialType(lowestHPAlly);

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

            // Cover
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cover4, PerkType.Cover, () => allyHPPercentage <= 60f))
            {
                return (Feat.Cover4, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cover3, PerkType.Cover, () => allyHPPercentage <= 60f))
            {
                return (Feat.Cover3, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cover2, PerkType.Cover, () => allyHPPercentage <= 60f))
            {
                return (Feat.Cover2, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cover1, PerkType.Cover, () => allyHPPercentage <= 60f))
            {
                return (Feat.Cover1, self);
            }

            // HP Restoration
            if (CheckIfCanUseFeat(self, self, Feat.Cure3, PerkType.Cure, () => hpPercentage <= 50f && selfRace != RacialType.Undead))
            {
                return (Feat.Cure3, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cure3, PerkType.Cure, () => allyHPPercentage <= 50f && lowestHPAllyRace != RacialType.Undead))
            {
                return (Feat.Cure3, lowestHPAlly);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Cure2, PerkType.Cure, () => hpPercentage <= 75f && selfRace != RacialType.Undead))
            {
                return (Feat.Cure2, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cure2, PerkType.Cure, () => allyHPPercentage <= 75f && lowestHPAllyRace != RacialType.Undead))
            {
                return (Feat.Cure2, lowestHPAlly);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Cure1, PerkType.Cure, () => hpPercentage <= 80f && selfRace != RacialType.Undead))
            {
                return (Feat.Cure1, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Cure1, PerkType.Cure, () => allyHPPercentage <= 80f && lowestHPAllyRace != RacialType.Undead))
            {
                return (Feat.Cure1, lowestHPAlly);
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
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Regen2, PerkType.Regen, () => allyHPPercentage <= 85f))
            {
                return (Feat.Regen2, lowestHPAlly);
            }
            if (CheckIfCanUseFeat(self, self, Feat.Regen1, PerkType.Regen, () => hpPercentage <= 90f))
            {
                return (Feat.Regen1, self);
            }
            if (CheckIfCanUseFeat(self, lowestHPAlly, Feat.Regen1, PerkType.Regen, () => allyHPPercentage <= 90f))
            {
                return (Feat.Regen1, lowestHPAlly);
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
            if (!GetIsObjectValid(target)) return false;

            var effectiveLevel = Perk.GetEffectivePerkLevel(creature, perkType);
            return Ability.CanUseAbility(creature, target, feat, effectiveLevel);
        }

    }
}
