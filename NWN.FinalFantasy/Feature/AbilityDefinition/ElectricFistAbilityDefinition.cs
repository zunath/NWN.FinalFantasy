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
    public class ElectricFistAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            ElectricFist1(builder);
            ElectricFist2(builder);
            ElectricFist3(builder);

            return builder.Build();
        }

        private static void ElectricFist1(AbilityBuilder builder)
        {
            builder.Create(Feat.ElectricFist1, PerkType.ElectricFist)
                .Name("Electric Fist I")
                .HasRecastDelay(RecastGroup.ElectricFist, 60f)
                .UsesActivationType(AbilityActivationType.Weapon)
                .RequirementStamina(3)
                .HasImpactAction((activator, target, level) =>
                {
                    var modifier = GetAbilityModifier(AbilityType.Wisdom, activator);
                    modifier = modifier > 0 ? modifier : 0;
                    var damage = Random.D4(2) + modifier;
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Electrical), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Hit_Electrical), target);

                    CombatPoint.AddCombatPoint(activator, target, SkillType.Chi, 3);
                    Enmity.ModifyEnmityOnAll(activator, 6 + damage);
                });
        }

        private static void ElectricFist2(AbilityBuilder builder)
        {
            builder.Create(Feat.ElectricFist2, PerkType.ElectricFist)
                .Name("Electric Fist II")
                .HasRecastDelay(RecastGroup.ElectricFist, 120f)
                .UsesActivationType(AbilityActivationType.Weapon)
                .RequirementStamina(6)
                .HasImpactAction((activator, target, level) =>
                {
                    var modifier = GetAbilityModifier(AbilityType.Wisdom, activator);
                    modifier = modifier > 0 ? modifier : 0;
                    var damage = Random.D8(2) + modifier;
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Electrical), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Hit_Electrical), target);

                    if (Random.D100(1) <= 50)
                    {
                        StatusEffect.Apply(activator, target, StatusEffectType.Static, 18f);
                    }

                    CombatPoint.AddCombatPoint(activator, target, SkillType.Chi, 3);
                    Enmity.ModifyEnmityOnAll(activator, 8 + damage);
                });
        }

        private static void ElectricFist3(AbilityBuilder builder)
        {
            builder.Create(Feat.ElectricFist3, PerkType.ElectricFist)
                .Name("Electric Fist III")
                .HasRecastDelay(RecastGroup.ElectricFist, 180f)
                .UsesActivationType(AbilityActivationType.Weapon)
                .RequirementStamina(14)
                .HasImpactAction((activator, target, level) =>
                {
                    var modifier = GetAbilityModifier(AbilityType.Wisdom, activator);
                    modifier = modifier > 0 ? modifier : 0;
                    var damage = Random.D8(3) + modifier;
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Electrical), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Hit_Electrical), target);
                    StatusEffect.Apply(activator, target, StatusEffectType.Static, 30f);

                    CombatPoint.AddCombatPoint(activator, target, SkillType.Chi, 3);
                    Enmity.ModifyEnmityOnAll(activator, 10 + damage);
                });
        }
    }
}
