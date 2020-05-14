using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
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
                    var baseDamage = Random.D6(2);
                    ApplyDamage(activator, target, baseDamage);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
                    Enmity.ModifyEnmity(activator, target, 5);
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
                    var baseDamage = Random.D8(2);
                    ApplyDamage(activator, target, baseDamage);
                    ApplyEffectToObject(DurationType.Temporary, EffectStunned(), target, 2f);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
                    Enmity.ModifyEnmity(activator, target, 10);
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
                    var baseDamage = Random.D12(2);
                    ApplyDamage(activator, target, baseDamage);
                    ApplyEffectToObject(DurationType.Temporary, EffectStunned(), target, 6f);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.BlackMagic, 3);
                    Enmity.ModifyEnmity(activator, target, 15);
                });
        }
    }
}
