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
    public class RecoveryStabAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            RecoveryStab1(builder);
            RecoveryStab2(builder);
            RecoveryStab3(builder);

            return builder.Build();
        }

        private static void RecoveryStab1(AbilityBuilder builder)
        {
            builder.Create(Feat.RecoveryStab1, PerkType.RecoveryStab)
                .Name("Recovery Stab I")
                .HasRecastDelay(RecastGroup.RecoveryStab, 60f)
                .RequirementStamina(8)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var bonus = GetAbilityModifier(AbilityType.Wisdom, activator);
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var amount = Random.D6(2) + bonus;

                        ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_S), member);
                    }
                });
        }

        private static void RecoveryStab2(AbilityBuilder builder)
        {
            builder.Create(Feat.RecoveryStab2, PerkType.RecoveryStab)
                .Name("Recovery Stab II")
                .HasRecastDelay(RecastGroup.RecoveryStab, 60f)
                .RequirementStamina(12)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var bonus = GetAbilityModifier(AbilityType.Wisdom, activator);
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var amount = Random.D8(3) + bonus;

                        ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_S), member);
                    }
                });
        }

        private static void RecoveryStab3(AbilityBuilder builder)
        {
            builder.Create(Feat.RecoveryStab3, PerkType.RecoveryStab)
                .Name("Recovery Stab III")
                .HasRecastDelay(RecastGroup.RecoveryStab, 60f)
                .RequirementStamina(16)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var bonus = GetAbilityModifier(AbilityType.Wisdom, activator);
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var amount = Random.D8(3) + bonus;
                        var duration = 24f;

                        if (StatusEffect.HasStatusEffect(activator, StatusEffectType.DeliberateStab))
                            duration *= 2f;

                        ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_S), member);
                        ApplyEffectToObject(DurationType.Temporary, EffectRegenerate(1, 6f), member, duration);
                    }
                });
        }
    }
}
