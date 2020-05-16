using System.Collections.Generic;
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
    public class ThunderAbilityDefinition : IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Thunder1(builder);
            Thunder2(builder);
            Thunder3(builder);

            return builder.Build();
        }

        private static int CalculateDamage(uint activator, uint target, int baseDamage)
        {
            var resistance = Resistance.GetResistance(target, ResistanceType.Thunder);
            var statAdjustment = 0.1f * (GetAbilityModifier(AbilityType.Intelligence, activator) - GetAbilityModifier(AbilityType.Intelligence, target));
            resistance += statAdjustment;
            var damage = (int)(baseDamage * resistance);

            if (damage < 1)
                damage = 1;

            return damage;
        }

        private static void AdjustResistances(uint target)
        {
            Resistance.ModifyResistance(target, ResistanceType.Fire, 0.1f);
            Resistance.ModifyResistance(target, ResistanceType.Ice, 0.05f);
            Resistance.ModifyResistance(target, ResistanceType.Thunder, -0.2f);
        }

        private static void ApplyDamage(uint activator, uint target, int baseDamage)
        {
            var damage = CalculateDamage(activator, target, baseDamage);

            AssignCommand(activator, () =>
            {
                ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Lightning_S), target);
                ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Electrical), target);
            });

            AdjustResistances(target);
        }

        private static void ApplyThunderEffects(uint activator, uint target, int baseDamage, int enmity, float stunLength)
        {
            var multiplier = 1;
            if (StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSeal))
                multiplier = 3;

            var damage = baseDamage * multiplier;
            ApplyDamage(activator, target, damage);

            if (stunLength > 0.0f)
            {
                ApplyEffectToObject(DurationType.Temporary, EffectStunned(), target, stunLength);
            }

            CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
            Enmity.ModifyEnmity(activator, target, enmity);
        }

        private static void ApplyAOEThunderEffects(uint activator, uint target, int baseDamage, int enmity, float stunLength)
        {
            if (!StatusEffect.HasStatusEffect(activator, StatusEffectType.ElementalSpread)) return;

            var nth = 1;
            var nearby = GetNearestCreature(Type.IsAlive, 1, target, nth);
            while (GetIsObjectValid(nearby))
            {
                if (target == nearby) continue;
                if (GetDistanceBetween(target, nearby) > 5.0f) break;

                ApplyThunderEffects(activator, nearby, baseDamage, enmity, stunLength);

                nth++;
                nearby = GetNearestCreature(Type.IsAlive, 1, target, nth);
            }

            StatusEffect.Remove(activator, StatusEffectType.ElementalSpread);
        }
        private static void Thunder1(AbilityBuilder builder)
        {
            builder.Create(Feat.Thunder1, PerkType.Thunder)
                .Name("Thunder I")
                .HasRecastDelay(RecastGroup.Thunder, 4f)
                .HasActivationDelay(4.0f)
                .RequirementMP(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyThunderEffects(activator, target, Random.D6(2), 5, 0.0f);
                    ApplyAOEThunderEffects(activator, target, Random.D6(2), 5, 0.0f);
                });
        }

        private static void Thunder2(AbilityBuilder builder)
        {
            builder.Create(Feat.Thunder2, PerkType.Thunder)
                .Name("Thunder II")
                .HasRecastDelay(RecastGroup.Thunder, 4f)
                .HasActivationDelay(4.0f)
                .RequirementMP(6)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyThunderEffects(activator, target, Random.D8(2), 10, 2f);
                    ApplyAOEThunderEffects(activator, target, Random.D8(2), 10, 2f);
                });
        }

        private static void Thunder3(AbilityBuilder builder)
        {
            builder.Create(Feat.Thunder3, PerkType.Thunder)
                .Name("Thunder III")
                .HasRecastDelay(RecastGroup.Thunder, 4f)
                .HasActivationDelay(4.0f)
                .RequirementMP(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyThunderEffects(activator, target, Random.D12(2), 15, 6f);
                    ApplyAOEThunderEffects(activator, target, Random.D12(2), 15, 6f);
                });
        }
    }
}
