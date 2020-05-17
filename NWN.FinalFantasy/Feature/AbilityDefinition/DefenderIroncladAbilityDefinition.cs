using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Feat = NWN.FinalFantasy.Core.NWScript.Enum.Feat;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class DefenderIroncladAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Defender1(builder);
            Defender2(builder);
            Defender3(builder);
            Ironclad1(builder);
            Ironclad2(builder);
            Ironclad3(builder);

            return builder.Build();
        }

        private static void Defender1(AbilityBuilder builder)
        {
            builder.Create(Feat.Defender1, PerkType.Defender)
                .Name("Defender I")
                .HasRecastDelay(RecastGroup.Defender, 300f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(target, 5.0f))
                    {
                        StatusEffect.Apply(activator, member, StatusEffectType.Defender1, 60f);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Death_Ward), member);
                    }
                });
        }

        private static void Defender2(AbilityBuilder builder)
        {
            builder.Create(Feat.Defender2, PerkType.Defender)
                .Name("Defender II")
                .HasRecastDelay(RecastGroup.Defender, 300f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(target, 5.0f))
                    {
                        StatusEffect.Apply(activator, member, StatusEffectType.Defender2, 60f);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Death_Ward), member);
                    }
                });
        }

        private static void Defender3(AbilityBuilder builder)
        {
            builder.Create(Feat.Defender3, PerkType.Defender)
                .Name("Defender III")
                .HasRecastDelay(RecastGroup.Defender, 300f)
                .RequirementStamina(25)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(target, 5.0f))
                    {
                        StatusEffect.Apply(activator, member, StatusEffectType.Defender3, 60f);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Death_Ward), member);
                    }
                });
        }

        private static void Ironclad1(AbilityBuilder builder)
        {
            builder.Create(Feat.Ironclad1, PerkType.Ironclad)
                .Name("Ironclad I")
                .HasRecastDelay(RecastGroup.Ironclad, 300f)
                .RequirementStamina(10)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Ironclad1, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Death_Ward), target);
                });
        }

        private static void Ironclad2(AbilityBuilder builder)
        {
            builder.Create(Feat.Ironclad2, PerkType.Ironclad)
                .Name("Ironclad II")
                .HasRecastDelay(RecastGroup.Ironclad, 300f)
                .RequirementStamina(14)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Ironclad2, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Death_Ward), target);
                });
        }

        private static void Ironclad3(AbilityBuilder builder)
        {
            builder.Create(Feat.Ironclad3, PerkType.Ironclad)
                .Name("Ironclad III")
                .HasRecastDelay(RecastGroup.Ironclad, 300f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Ironclad3, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Death_Ward), target);
                });
        }
    }
}
