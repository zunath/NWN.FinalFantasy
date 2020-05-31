using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class LifeStealAbilityDefinition : IAbilityListDefinition
    {
        private const string LifeStealVariableName = "LIFE_STEAL";
        private const string LifeStealActivatorVariableName = "LIFE_STEAL_ACTIVATOR";

        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            LifeSteal1(builder);
            LifeSteal2(builder);

            return builder.Build();
        }

        private static void LifeSteal1(AbilityBuilder builder)
        {
            builder.Create(Feat.LifeSteal1, PerkType.LifeSteal)
                .Name("Life Steal I")
                .HasRecastDelay(RecastGroup.LifeSteal, 120f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var percentRecovery = 0.25f;
                    SetLocalFloat(target, LifeStealVariableName, percentRecovery);
                    SetLocalObject(target, LifeStealActivatorVariableName, activator);
                });
        }

        private static void LifeSteal2(AbilityBuilder builder)
        {
            builder.Create(Feat.LifeSteal2, PerkType.LifeSteal)
                .Name("Life Steal II")
                .HasRecastDelay(RecastGroup.LifeSteal, 120f)
                .RequirementStamina(30)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var percentRecovery = 0.50f;
                    SetLocalFloat(target, LifeStealVariableName, percentRecovery);
                    SetLocalObject(target, LifeStealActivatorVariableName, activator);
                });
        }

        /// <summary>
        /// When damage is applied, if the target has a Life Steal variable set,
        /// that percent of the damage dealt is restored on the activator.
        /// </summary>
        [NWNEventHandler("on_nwnx_dmg")]
        public static void ApplyLifeStealRecovery()
        {
            var target = OBJECT_SELF;
            var lifeStealAmount = GetLocalFloat(target, LifeStealVariableName);
            var lifeStealActivator = GetLocalObject(target, LifeStealActivatorVariableName);
            if (lifeStealAmount <= 0.0f) return;

            var damageDetails = Damage.GetDamageEventData();
            if (lifeStealActivator != damageDetails.Damager) return;

            var recoveryAmount = (int) (damageDetails.Total * lifeStealAmount);
            ApplyEffectToObject(DurationType.Instant, EffectHeal(recoveryAmount), lifeStealActivator);
            ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_S), lifeStealActivator);

            Enmity.ModifyEnmity(lifeStealActivator, target, recoveryAmount + 6);
            CombatPoint.AddCombatPoint(lifeStealActivator, target, SkillType.Thievery, 2);

            DeleteLocalFloat(target, LifeStealVariableName);
            DeleteLocalObject(target, LifeStealActivatorVariableName);
        }
    }
}
