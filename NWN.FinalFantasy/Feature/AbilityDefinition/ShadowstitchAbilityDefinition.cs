using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class ShadowstitchAbilityDefinition : IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Shadowstitch1(builder);
            Shadowstitch2(builder);

            return builder.Build();
        }

        private static void Shadowstitch1(AbilityBuilder builder)
        {
            builder.Create(Feat.Shadowstitch1, PerkType.Shadowstitch)
                .Name("Shadowstitch I")
                .HasRecastDelay(RecastGroup.Shadowstitch, 60f)
                .RequirementStamina(10)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var duration = 12f;
                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Aura_Magenta), target, duration);
                    ApplyEffectToObject(DurationType.Temporary, EffectCutsceneImmobilize(), target, duration);
                });
        }


        private static void Shadowstitch2(AbilityBuilder builder)
        {
            builder.Create(Feat.Shadowstitch2, PerkType.Shadowstitch)
                .Name("Shadowstitch II")
                .HasRecastDelay(RecastGroup.Shadowstitch, 60f)
                .RequirementStamina(18)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var duration = 24f;
                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Aura_Magenta), target, duration);
                    ApplyEffectToObject(DurationType.Temporary, EffectCutsceneImmobilize(), target, duration);
                });
        }
    }
}
