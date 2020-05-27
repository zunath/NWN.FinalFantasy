using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class HideAbilityDefinition : IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Hide1(builder);
            Hide2(builder);

            return builder.Build();
        }

        private static void Hide1(AbilityBuilder builder)
        {
            builder.Create(Feat.Hide1, PerkType.Hide)
                .Name("Hide I")
                .HasRecastDelay(RecastGroup.Hide, 600f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2.0f)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (GetIsInCombat(activator))
                    {
                        return "You cannot hide during combat.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var bonusDuration = GetAbilityModifier(AbilityType.Dexterity, activator) * 5;
                    var duration = Random.NextFloat(120f, 300f) + bonusDuration;
                    ApplyEffectToObject(DurationType.Temporary, EffectInvisibility(InvisibilityType.Normal), target, duration);
                });
        }

        private static void Hide2(AbilityBuilder builder)
        {
            builder.Create(Feat.Hide2, PerkType.Hide)
                .Name("Hide II")
                .HasRecastDelay(RecastGroup.Hide, 600f)
                .RequirementStamina(25)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2.0f)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (GetIsInCombat(activator))
                    {
                        return "You cannot hide during combat.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var bonusDuration = GetAbilityModifier(AbilityType.Dexterity, activator) * 5;
                    var duration = Random.NextFloat(240f, 300f) + bonusDuration;
                    ApplyEffectToObject(DurationType.Temporary, EffectInvisibility(InvisibilityType.Normal), target, duration);
                });
        }
    }
}