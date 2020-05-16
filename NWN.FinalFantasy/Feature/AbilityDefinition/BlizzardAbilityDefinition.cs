using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Creature;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class BlizzardAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Blizzard1(builder);
            Blizzard2(builder);
            Blizzard3(builder);

            return builder.Build();
        }

        private static int CalculateDamage(uint activator, uint target, int baseDamage)
        {
            var resistance = Resistance.GetResistance(target, ResistanceType.Ice);
            var statAdjustment = 0.1f * (GetAbilityModifier(AbilityType.Intelligence, activator) - GetAbilityModifier(AbilityType.Intelligence, target));
            resistance += statAdjustment;
            var damage = (int)(baseDamage * resistance);

            if (damage < 1)
                damage = 1;

            return damage;
        }

        private static void AdjustResistances(uint target)
        {
            Resistance.ModifyResistance(target, ResistanceType.Fire, 0.05f);
            Resistance.ModifyResistance(target, ResistanceType.Ice, -0.2f);
            Resistance.ModifyResistance(target, ResistanceType.Thunder, 0.1f);
        }

        private static void ApplyDamage(uint activator, uint target, int baseDamage)
        {
            var damage = CalculateDamage(activator, target, baseDamage);

            AssignCommand(activator, () =>
            {
                ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Frost_L), target);
                ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Cold), target);
            });

            AdjustResistances(target);
        }

        private static void ApplyBlizzardEffects(uint activator, uint target, int baseDamage, int enmity, float slowLength)
        {
            var multiplier = 1;
            if (StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSeal))
                multiplier = 3;

            var damage = baseDamage * multiplier;
            ApplyDamage(activator, target, damage);

            if (slowLength > 0.0f)
            {
                ApplyEffectToObject(DurationType.Temporary, EffectMovementSpeedDecrease(25), target, slowLength);
            }

            CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
            Enmity.ModifyEnmity(activator, target, enmity);
        }

        private static void ApplyAOEBlizzardEffects(uint activator, uint target, int baseDamage, int enmity, float slowLength)
        {
            if (!StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSpread)) return;

            var nth = 1;
            var nearby = GetNearestCreature(Type.IsAlive, 1, target, nth);
            while (GetIsObjectValid(nearby))
            {
                if (target == nearby) continue;
                if (GetDistanceBetween(target, nearby) > 5.0f) break;

                ApplyBlizzardEffects(activator, nearby, baseDamage, enmity, slowLength);

                nth++;
                nearby = GetNearestCreature(Type.IsAlive, 1, target, nth);
            }

            StatusEffect.Remove(activator, StatusEffectType.ElementalSpread);
        }

        private static void Blizzard1(AbilityBuilder builder)
        {
            builder.Create(Feat.Blizzard1, PerkType.Blizzard)
                .Name("Blizzard I")
                .HasRecastDelay(RecastGroup.Blizzard, 2f)
                .HasActivationDelay(2.0f)
                .RequirementMP(2)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyBlizzardEffects(activator, target, Random.D6(2), 5, 0.0f);
                    ApplyAOEBlizzardEffects(activator, target, Random.D6(2), 5, 0.0f);
                });
        }

        private static void Blizzard2(AbilityBuilder builder)
        {
            builder.Create(Feat.Blizzard2, PerkType.Blizzard)
                .Name("Blizzard II")
                .HasRecastDelay(RecastGroup.Blizzard, 2f)
                .HasActivationDelay(2.0f)
                .RequirementMP(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyBlizzardEffects(activator, target, Random.D8(2), 10, 15f);
                    ApplyAOEBlizzardEffects(activator, target, Random.D8(2), 10, 15f);
                });
        }

        private static void Blizzard3(AbilityBuilder builder)
        {
            builder.Create(Feat.Blizzard3, PerkType.Blizzard)
                .Name("Blizzard III")
                .HasRecastDelay(RecastGroup.Blizzard, 2f)
                .HasActivationDelay(2.0f)
                .RequirementMP(6)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyBlizzardEffects(activator, target, Random.D8(2), 10, 30f);
                    ApplyAOEBlizzardEffects(activator, target, Random.D8(2), 10, 30f);
                });
        }
    }
}
