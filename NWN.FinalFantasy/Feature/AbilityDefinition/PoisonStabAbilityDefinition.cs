using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class PoisonStabAbilityDefinition : IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            PoisonStab1(builder);
            PoisonStab2(builder);

            return builder.Build();
        }

        private static float CalculateDuration(uint activator)
        {
            const float BaseDuration = 60f;
            var duration = BaseDuration;

            if (StatusEffect.HasStatusEffect(activator, StatusEffectType.DeliberateStab))
                duration *= 2;

            return duration;
        }

        private static void PoisonStab1(AbilityBuilder builder)
        {
            builder.Create(Feat.PoisonStab1, PerkType.PiercingStab)
                .Name("Poison Stab I")
                .HasRecastDelay(RecastGroup.PoisonStab, 60f)
                .RequirementStamina(8)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var duration = CalculateDuration(activator);
                    StatusEffect.Apply(activator, target, StatusEffectType.Poison1, duration);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Disease_S), target);

                    CombatPoint.AddCombatPoint(activator, target, SkillType.RedMagic, 2);
                    Enmity.ModifyEnmity(activator, target, 10);
                });
        }

        private static void PoisonStab2(AbilityBuilder builder)
        {
            builder.Create(Feat.PoisonStab2, PerkType.PiercingStab)
                .Name("Poison Stab II")
                .HasRecastDelay(RecastGroup.PoisonStab, 60f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var duration = CalculateDuration(activator);
                    StatusEffect.Apply(activator, target, StatusEffectType.Poison2, duration);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Disease_S), target);

                    CombatPoint.AddCombatPoint(activator, target, SkillType.RedMagic, 2);
                    Enmity.ModifyEnmity(activator, target, 12);
                });
        }
    }
}
