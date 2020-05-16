using System;
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
    public class FireAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Fire1(builder);
            Fire2(builder);
            Fire3(builder);

            return builder.Build();
        }

        private static int CalculateDamage(uint activator, uint target, int baseDamage)
        {
            var resistance = Resistance.GetResistance(target, ResistanceType.Fire);
            var statAdjustment = 0.1f * (GetAbilityModifier(AbilityType.Intelligence, activator) - GetAbilityModifier(AbilityType.Intelligence, target));
            resistance += statAdjustment;
            var damage = (int)(baseDamage * resistance);

            if (damage < 1)
                damage = 1;

            return damage;
        }

        private static void AdjustResistances(uint target)
        {
            Resistance.ModifyResistance(target, ResistanceType.Fire, -0.2f);
            Resistance.ModifyResistance(target, ResistanceType.Ice, 0.1f);
            Resistance.ModifyResistance(target, ResistanceType.Thunder, 0.05f);
        }

        private static void ApplyDamage(uint activator, uint target, int baseDamage)
        {
            var damage = CalculateDamage(activator, target, baseDamage);

            AssignCommand(activator, () =>
            {
                ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Flame_S), target);
                ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Fire), target);
            });

            AdjustResistances(target);
        }

        private static void Fire1(AbilityBuilder builder)
        {
            builder.Create(Feat.Fire1, PerkType.Fire)
                .Name("Fire I")
                .HasRecastDelay(RecastGroup.Fire, 3f)
                .HasActivationDelay(3.0f)
                .RequirementMP(3)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var multiplier = 1;
                    if (StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSeal))
                        multiplier = 3;

                    var baseDamage = Random.D6(2) * multiplier;
                    ApplyDamage(activator, target, baseDamage);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
                    Enmity.ModifyEnmity(activator, target, 5);
                });
        }

        private static void Fire2(AbilityBuilder builder)
        {
            builder.Create(Feat.Fire2, PerkType.Fire)
                .Name("Fire II")
                .HasRecastDelay(RecastGroup.Fire, 3f)
                .HasActivationDelay(3.0f)
                .RequirementMP(5)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var multiplier = 1;
                    if (StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSeal))
                        multiplier = 3;

                    var baseDamage = Random.D8(2) * multiplier;
                    ApplyDamage(activator, target, baseDamage);
                    StatusEffect.Apply(activator, target, StatusEffectType.Burn, 18.0f);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
                    Enmity.ModifyEnmity(activator, target, 10);
                });
        }

        private static void Fire3(AbilityBuilder builder)
        {
            builder.Create(Feat.Fire3, PerkType.Fire)
                .Name("Fire III")
                .HasRecastDelay(RecastGroup.Fire, 3f)
                .HasActivationDelay(3.0f)
                .RequirementMP(7)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var multiplier = 1;
                    if (StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSeal))
                        multiplier = 3;

                    var baseDamage = Random.D12(2) * multiplier;
                    ApplyDamage(activator, target, baseDamage);
                    StatusEffect.Apply(activator, target, StatusEffectType.Burn, 30.0f);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
                    Enmity.ModifyEnmity(activator, target, 15);
                });
        }
    }
}
