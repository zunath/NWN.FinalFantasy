using System;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
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
            if (GetIsInCombat(self) && target != GetAttackTarget(self))
            {
                AssignCommand(self, () =>
                {
                    ClearAllActions();
                    ActionAttack(target);
                });
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
    }
}
