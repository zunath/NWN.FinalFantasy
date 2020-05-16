using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class ValorAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Valor1(builder);
            Valor2(builder);

            return builder.Build();
        }

        private static void Valor1(AbilityBuilder builder)
        {
            builder.Create(Feat.Valor1, PerkType.Valor)
                .Name("Valor I")
                .HasRecastDelay(RecastGroup.Valor, 360f)
                .HasActivationDelay(4f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(25)
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var hpRecover = (int)(GetMaxHitPoints(member) * 0.2f);
                        ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), member);
                    }

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 5);
                    Enmity.ModifyEnmityOnAll(activator, 25);
                });
        }

        private static void Valor2(AbilityBuilder builder)
        {
            builder.Create(Feat.Valor2, PerkType.Valor)
                .Name("Valor II")
                .HasRecastDelay(RecastGroup.Valor, 600f)
                .HasActivationDelay(4f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(40)
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var hpRecover = (int)(GetMaxHitPoints(member) * 0.4f);
                        ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Restoration), member);
                    }

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 5);
                    Enmity.ModifyEnmityOnAll(activator, 70);
                });
        }
    }
}
