using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class RaiseAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Raise(builder);

            return builder.Build();
        }

        private static void Raise(AbilityBuilder builder)
        {
            builder.Create(Feat.Raise, PerkType.Raise)
                .Name("Raise I")
                .HasRecastDelay(RecastGroup.Raise, 60f)
                .HasActivationDelay(15f)
                .RequirementMP(25)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (GetCurrentHitPoints(target) > -11 ||
                        !GetIsDead(target))
                    {
                        return "Your target is still alive.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectResurrection(), target);
                });
        }
    }
}
