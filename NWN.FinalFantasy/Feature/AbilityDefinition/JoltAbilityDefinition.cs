using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class JoltAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Jolt1(builder);
            Jolt2(builder);
            Jolt3(builder);

            return builder.Build();
        }

        private static void Jolt1(AbilityBuilder builder)
        {
            builder.Create(Feat.Jolt1, PerkType.Jolt)
                .Name("Jolt I")
                .HasRecastDelay(RecastGroup.Jolt, 4f)
                .RequirementMP(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2f)
                .HasImpactAction((activator, target, level) =>
                {
                    var damage = Random.D6(1);

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Hit_Electrical), target);
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage), target);

                    Enmity.ModifyEnmity(activator, target, damage + 4);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.RedMagic, 3);
                });
        }

        private static void Jolt2(AbilityBuilder builder)
        {
            builder.Create(Feat.Jolt2, PerkType.Jolt)
                .Name("Jolt II")
                .HasRecastDelay(RecastGroup.Jolt, 4f)
                .RequirementMP(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2f)
                .HasImpactAction((activator, target, level) =>
                {
                    var damage = Random.D6(2);

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Hit_Electrical), target);
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage), target);

                    Enmity.ModifyEnmity(activator, target, damage + 6);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.RedMagic, 3);
                });
        }

        private static void Jolt3(AbilityBuilder builder)
        {
            builder.Create(Feat.Jolt3, PerkType.Jolt)
                .Name("Jolt III")
                .HasRecastDelay(RecastGroup.Jolt, 4f)
                .RequirementMP(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2f)
                .HasImpactAction((activator, target, level) =>
                {
                    var damage = Random.D10(2);

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Hit_Electrical), target);
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage), target);

                    Enmity.ModifyEnmity(activator, target, damage + 8);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.RedMagic, 3);
                });
        }
    }
}
