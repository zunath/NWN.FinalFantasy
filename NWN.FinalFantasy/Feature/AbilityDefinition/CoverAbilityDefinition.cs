using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class CoverAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Cover1(builder);
            Cover2(builder);
            Cover3(builder);
            Cover4(builder);

            return builder.Build();
        }

        private static void Cover1(AbilityBuilder builder)
        {
            builder.Create(Feat.Cover1, PerkType.Cover)
                .Name("Cover I")
                .HasRecastDelay(RecastGroup.Cover, 120f)
                .RequirementStamina(6)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating(VisualEffect.None)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (activator == target)
                    {
                        return "You cannot Cover yourself.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var length = 60f + GetAbilityModifier(AbilityType.Constitution, activator) * 2f;
                    StatusEffect.Apply(activator, target, StatusEffectType.Cover1, length);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Dur_Prot_Epic_Armor), target);

                    Enmity.ModifyEnmityOnAll(activator, 10);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 3);
                });
        }

        private static void Cover2(AbilityBuilder builder)
        {
            builder.Create(Feat.Cover2, PerkType.Cover)
                .Name("Cover II")
                .HasRecastDelay(RecastGroup.Cover, 120f)
                .RequirementStamina(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating(VisualEffect.None)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (activator == target)
                    {
                        return "You cannot Cover yourself.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var length = 60f + GetAbilityModifier(AbilityType.Constitution, activator) * 2f;
                    StatusEffect.Apply(activator, target, StatusEffectType.Cover2, length);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Dur_Prot_Epic_Armor), target);

                    Enmity.ModifyEnmityOnAll(activator, 10);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 3);
                });
        }

        private static void Cover3(AbilityBuilder builder)
        {
            builder.Create(Feat.Cover3, PerkType.Cover)
                .Name("Cover III")
                .HasRecastDelay(RecastGroup.Cover, 120f)
                .RequirementStamina(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating(VisualEffect.None)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (activator == target)
                    {
                        return "You cannot Cover yourself.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var length = 60f + GetAbilityModifier(AbilityType.Constitution, activator) * 2f;
                    StatusEffect.Apply(activator, target, StatusEffectType.Cover3, length);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Dur_Prot_Epic_Armor), target);

                    Enmity.ModifyEnmityOnAll(activator, 10);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 3);
                });
        }

        private static void Cover4(AbilityBuilder builder)
        {
            builder.Create(Feat.Cover4, PerkType.Cover)
                .Name("Cover IV")
                .HasRecastDelay(RecastGroup.Cover, 120f)
                .RequirementStamina(14)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating(VisualEffect.None)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (activator == target)
                    {
                        return "You cannot Cover yourself.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var length = 60f + GetAbilityModifier(AbilityType.Constitution, activator) * 2f;
                    StatusEffect.Apply(activator, target, StatusEffectType.Cover4, length);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Dur_Prot_Epic_Armor), target);

                    Enmity.ModifyEnmityOnAll(activator, 10);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 3);
                });
        }
    }
}
