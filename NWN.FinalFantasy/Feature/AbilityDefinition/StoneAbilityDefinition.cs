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
    public class StoneAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Stone1(builder);
            Stone2(builder);
            Stone3(builder);

            return builder.Build();
        }

        private static int CalculateDamage(uint activator, uint target, int baseDamage)
        {
            var resistance = Resistance.GetResistance(target, ResistanceType.Fire);
            var statAdjustment = 0.1f * (GetAbilityModifier(AbilityType.Wisdom, activator) - GetAbilityModifier(AbilityType.Wisdom, target));
            resistance += statAdjustment;
            var damage = (int)(baseDamage * resistance);

            if (damage < 1)
                damage = 1;

            return damage;
        }

        private static void AdjustResistances(uint target)
        {
            Resistance.ModifyResistance(target, ResistanceType.Earth, -0.2f);
        }

        private static void ApplyDamage(uint activator, uint target, int baseDamage)
        {
            var damage = CalculateDamage(activator, target, baseDamage);

            AssignCommand(activator, () =>
            {
                ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Fnf_Gas_Explosion_Nature), target);
                ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Acid), target);
            });

            AdjustResistances(target);
        }

        private static void ApplyStoneEffects(uint activator, uint target, int damage, int enmity)
        {
            ApplyDamage(activator, target, damage);
            CombatPoint.AddCombatPoint(activator, target, SkillType.WhiteMagic, 3);
            Enmity.ModifyEnmity(activator, target, enmity);
        }

        private static void Stone1(AbilityBuilder builder)
        {
            builder.Create(Feat.Stone1, PerkType.Stone)
                .Name("Stone I")
                .HasRecastDelay(RecastGroup.Stone, 4f)
                .HasActivationDelay(2.0f)
                .RequirementMP(3)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyStoneEffects(activator, target, Random.D6(1), 4);
                });
        }

        private static void Stone2(AbilityBuilder builder)
        {
            builder.Create(Feat.Stone2, PerkType.Stone)
                .Name("Stone II")
                .HasRecastDelay(RecastGroup.Stone, 4f)
                .HasActivationDelay(2.0f)
                .RequirementMP(5)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyStoneEffects(activator, target, Random.D6(2), 9);
                });
        }

        private static void Stone3(AbilityBuilder builder)
        {
            builder.Create(Feat.Stone3, PerkType.Stone)
                .Name("Stone III")
                .HasRecastDelay(RecastGroup.Stone, 4f)
                .HasActivationDelay(2.0f)
                .RequirementMP(7)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyStoneEffects(activator, target, Random.D10(2), 14);
                });
        }
    }
}
