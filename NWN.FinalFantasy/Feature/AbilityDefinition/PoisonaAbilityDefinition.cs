using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class PoisonaAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Poisona(builder);

            return builder.Build();
        }

        private static void Poisona(AbilityBuilder builder)
        {
            builder.Create(Feat.Poisona, PerkType.Poisona)
                .Name("Poisona")
                .HasRecastDelay(RecastGroup.Raise, 60f)
                .HasActivationDelay(1f)
                .RequirementMP(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (!StatusEffect.HasStatusEffect(target, StatusEffectType.Poison1) &&
                        !StatusEffect.HasStatusEffect(target, StatusEffectType.Poison2) &&
                        !StatusEffect.HasStatusEffect(target, StatusEffectType.Poison3))
                    {
                        return "Your target is not poisoned.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Remove(target, StatusEffectType.Poison1);
                    StatusEffect.Remove(target, StatusEffectType.Poison2);
                    StatusEffect.Remove(target, StatusEffectType.Poison3);

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Head_Nature), target);
                });
        }
    }
}
