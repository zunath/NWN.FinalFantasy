using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class SneakAttackAbilityDefinition: IAbilityListDefinition
    {
        private const string SneakAttackVariableName = "SNEAK_ATTACK";

        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            SneakAttack1(builder);
            SneakAttack2(builder);

            return builder.Build();
        }

        /// <summary>
        /// Checks if the activator's facing is within +/- 20 degrees of the target's facing.
        /// </summary>
        /// <param name="activator">The activator of the Sneak Attack ability.</param>
        /// <param name="target">The target of the Sneak Attack ability.</param>
        /// <returns>true if activator is behind target, false otherwise.</returns>
        private static bool IsBehindTarget(uint activator, uint target)
        {
            var minimum = GetFacing(activator) - 20;
            var maximum = GetFacing(activator + 20);

            var targetFacing = GetFacing(target);

            return targetFacing >= minimum && targetFacing <= maximum;
        }

        private static void SneakAttack1(AbilityBuilder builder)
        {
            builder.Create(Feat.SneakAttack1, PerkType.SneakAttack)
                .Name("Sneak Attack I")
                .HasRecastDelay(RecastGroup.SneakAttack, 60f)
                .RequirementStamina(10)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var percentIncrease = 0.10f;
                    if (IsBehindTarget(activator, target))
                    {
                        percentIncrease = 0.25f;
                    }

                    SetLocalFloat(target, SneakAttackVariableName, percentIncrease);
                });
        }

        private static void SneakAttack2(AbilityBuilder builder)
        {
            builder.Create(Feat.SneakAttack2, PerkType.SneakAttack)
                .Name("Sneak Attack II")
                .HasRecastDelay(RecastGroup.SneakAttack, 60f)
                .RequirementStamina(18)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var percentIncrease = 0.15f;
                    if (IsBehindTarget(activator, target))
                    {
                        percentIncrease = 0.50f;
                    }

                    SetLocalFloat(target, SneakAttackVariableName, percentIncrease);
                });
        }

        /// <summary>
        /// When damage is applied, if the target has a Sneak Attack variable set,
        /// damage is increased by that amount.
        /// </summary>
        [NWNEventHandler("on_nwnx_dmg")]
        public static void ApplySneakAttackDamage()
        {
            var target = OBJECT_SELF;
            var sneakAttackBonus = GetLocalFloat(target, SneakAttackVariableName);
            if (sneakAttackBonus <= 0.0f) return;
            
            var damageDetails = Damage.GetDamageEventData();
            damageDetails.AdjustAllByPercent(sneakAttackBonus);
            
            DeleteLocalFloat(target, SneakAttackVariableName);
        }
    }
}
