using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class InnerHealingAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            InnerHealing1(builder);
            InnerHealing2(builder);
            InnerHealing3(builder);
            InnerHealing4(builder);
            InnerHealing5(builder);

            return builder.Build();
        }

        private static void InnerHealing1(AbilityBuilder builder)
        {
            builder.Create(Feat.InnerHealing1, PerkType.InnerHealing)
                .Name("Inner Healing I")
                .HasRecastDelay(RecastGroup.InnerHealing, 180f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(10)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = (int)(GetMaxHitPoints(target) * 0.1f);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 5);
                    Enmity.ModifyEnmityOnAll(activator, 10);
                });
        }

        private static void InnerHealing2(AbilityBuilder builder)
        {
            builder.Create(Feat.InnerHealing2, PerkType.InnerHealing)
                .Name("Inner Healing II")
                .HasRecastDelay(RecastGroup.InnerHealing, 180f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(15)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = (int)(GetMaxHitPoints(target) * 0.2f);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 6);
                    Enmity.ModifyEnmityOnAll(activator, 15);
                });
        }

        private static void InnerHealing3(AbilityBuilder builder)
        {
            builder.Create(Feat.InnerHealing3, PerkType.InnerHealing)
                .Name("Inner Healing III")
                .HasRecastDelay(RecastGroup.InnerHealing, 180f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(20)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = (int)(GetMaxHitPoints(target) * 0.3f);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 7);
                    Enmity.ModifyEnmityOnAll(activator, 20);
                });
        }

        private static void InnerHealing4(AbilityBuilder builder)
        {
            builder.Create(Feat.InnerHealing4, PerkType.InnerHealing)
                .Name("Inner Healing IV")
                .HasRecastDelay(RecastGroup.InnerHealing, 180f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(25)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = (int)(GetMaxHitPoints(target) * 0.4f);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 8);
                    Enmity.ModifyEnmityOnAll(activator, 25);
                });
        }

        private static void InnerHealing5(AbilityBuilder builder)
        {
            builder.Create(Feat.InnerHealing5, PerkType.InnerHealing)
                .Name("Inner Healing V")
                .HasRecastDelay(RecastGroup.InnerHealing, 180f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(30)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = (int)(GetMaxHitPoints(target) * 0.5f);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 9);
                    Enmity.ModifyEnmityOnAll(activator, 30);
                });
        }
    }
}
