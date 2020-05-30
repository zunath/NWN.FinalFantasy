using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class WaspStingAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            WaspSting1(builder);
            WaspSting2(builder);
            WaspSting3(builder);

            return builder.Build();
        }

        private static void WaspSting1(AbilityBuilder builder)
        {
            builder.Create(Feat.WaspSting1, PerkType.WaspSting)
                .Name("Wasp Sting I")
                .HasRecastDelay(RecastGroup.WaspSting, 60f)
                .RequirementStamina(5)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Poison_S), target);
                    StatusEffect.Apply(activator, target, StatusEffectType.Poison1, 24f);
                });
        }


        private static void WaspSting2(AbilityBuilder builder)
        {
            builder.Create(Feat.WaspSting2, PerkType.WaspSting)
                .Name("Wasp Sting II")
                .HasRecastDelay(RecastGroup.WaspSting, 60f)
                .RequirementStamina(8)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Poison_S), target);
                    StatusEffect.Apply(activator, target, StatusEffectType.Poison2, 24f);
                });
        }


        private static void WaspSting3(AbilityBuilder builder)
        {
            builder.Create(Feat.WaspSting3, PerkType.WaspSting)
                .Name("Wasp Sting III")
                .HasRecastDelay(RecastGroup.WaspSting, 60f)
                .RequirementStamina(12)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Poison_S), target);
                    StatusEffect.Apply(activator, target, StatusEffectType.Poison3, 24f);
                });
        }

    }
}
