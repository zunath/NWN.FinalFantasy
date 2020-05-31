using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class RegenAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Regen1(builder);
            Regen2(builder);

            return builder.Build();
        }

        private static void Regen1(AbilityBuilder builder)
        {
            builder.Create(Feat.Regen1, PerkType.Regen)
                .Name("Regen I")
                .HasRecastDelay(RecastGroup.Regen, 12f)
                .HasActivationDelay(4f)
                .RequirementMP(6)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectRegenerate(1, 6f), target, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Holy_Aid), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                    Enmity.ModifyEnmityOnAll(activator, 6);
                });
        }

        private static void Regen2(AbilityBuilder builder)
        {
            builder.Create(Feat.Regen1, PerkType.Regen)
                .Name("Regen II")
                .HasRecastDelay(RecastGroup.Regen, 12f)
                .HasActivationDelay(4f)
                .RequirementMP(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectRegenerate(2, 6f), target, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Holy_Aid), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                    Enmity.ModifyEnmityOnAll(activator, 6);
                });
        }
    }
}
