using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;
using Type = NWN.FinalFantasy.Core.NWScript.Enum.Creature.Type;

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

        private static void ApplyFireEffects(uint activator, uint target, int baseDamage, int enmity, float burnLength)
        {
            var multiplier = 1;
            if (StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSeal))
                multiplier = 3;

            var damage = baseDamage * multiplier;
            ApplyDamage(activator, target, damage);

            if (burnLength > 0.0f)
            {
                StatusEffect.Apply(activator, target, StatusEffectType.Burn, burnLength);
            }

            CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
            Enmity.ModifyEnmity(activator, target, enmity);
        }

        private static void ApplyAOEFireEffects(uint activator, uint target, int baseDamage, int enmity, float burnLength)
        {
            if (!StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSpread)) return;

            var nth = 1;
            var nearby = GetNearestCreature(Type.IsAlive, 1, target, nth);
            while (GetIsObjectValid(nearby))
            {
                if (target == nearby) continue;
                if (GetDistanceBetween(target, nearby) > 5.0f) break;

                ApplyFireEffects(activator, nearby, baseDamage, enmity, burnLength);

                nth++;
                nearby = GetNearestCreature(Type.IsAlive, 1, target, nth);
            }

            StatusEffect.Remove(activator, StatusEffectType.ElementalSpread);
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
                    ApplyFireEffects(activator, target, Random.D6(2), 5, 0.0f);
                    ApplyAOEFireEffects(activator, target, Random.D6(2), 5, 0.0f);
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
                    ApplyFireEffects(activator, target, Random.D8(2), 10, 18.0f);
                    ApplyAOEFireEffects(activator, target, Random.D8(2), 10, 18.0f);
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
                    ApplyFireEffects(activator, target, Random.D12(2), 15, 30f);
                    ApplyAOEFireEffects(activator, target, Random.D12(2), 15, 30f);
                });
        }
    }
}
