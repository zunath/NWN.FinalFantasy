using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class PiercingStabAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            PiercingStab1(builder);
            PiercingStab2(builder);
            PiercingStab3(builder);

            return builder.Build();
        }

        private static void PiercingStab1(AbilityBuilder builder)
        {
            builder.Create(Feat.PiercingStab1, PerkType.PiercingStab)
                .Name("Piercing Stab I")
                .HasRecastDelay(RecastGroup.PiercingStab, 60f)
                .RequirementStamina(5)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Bleed1, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Chunk_Red_Small), target);
                });
        }

        private static void PiercingStab2(AbilityBuilder builder)
        {
            builder.Create(Feat.PiercingStab2, PerkType.PiercingStab)
                .Name("Piercing Stab II")
                .HasRecastDelay(RecastGroup.PiercingStab, 60f)
                .RequirementStamina(10)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Bleed2, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Chunk_Red_Small), target);
                });
        }

        private static void PiercingStab3(AbilityBuilder builder)
        {
            builder.Create(Feat.PiercingStab3, PerkType.PiercingStab)
                .Name("Piercing Stab III")
                .HasRecastDelay(RecastGroup.PiercingStab, 60f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Bleed3, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Chunk_Red_Small), target);
                });
        }
    }
}
