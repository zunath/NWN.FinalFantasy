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

            //Console.WriteLine($"target = {GetName(highest.Key)}, enmity = {highest.Value}"); // todo debugging
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
        private static bool CheckIfCanUseFeat(uint creature, uint target, Feat feat, PerkType perkType, Func<bool> condition)
        {
            if (!GetHasFeat(feat, creature)) return false;
            if (!condition()) return false;

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

            if (CheckIfCanUseFeat(self, self, Feat.Cure3, PerkType.Cure, () => hpPercentage <= 50f))
            {
                return (Feat.Cure3, self);
            }
            else if (CheckIfCanUseFeat(self, target, Feat.Fire3, PerkType.Fire, () => true))
            {
                return (Feat.Fire3, target);
            }

            return (Feat.Invalid, OBJECT_INVALID);
        }

    }
}
