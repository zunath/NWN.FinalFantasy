using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class SubtleBlowAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            SubtleBlow1(builder);
            SubtleBlow2(builder);

            return builder.Build();
        }

        private static void SubtleBlow1(AbilityBuilder builder)
        {
            builder.Create(Feat.SubtleBlow1, PerkType.SubtleBlow)
                .Name("Subtle Blow I")
                .HasRecastDelay(RecastGroup.SubtleBlow, 300f)
                .RequirementStamina(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.SubtleBlow1, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Ac_Bonus), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 3);
                    Enmity.ModifyEnmityOnAll(activator, 6);
                });
        }

        private static void SubtleBlow2(AbilityBuilder builder)
        {
            builder.Create(Feat.SubtleBlow2, PerkType.SubtleBlow)
                .Name("Subtle Blow II")
                .HasRecastDelay(RecastGroup.SubtleBlow, 300f)
                .RequirementStamina(7)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.SubtleBlow2, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Ac_Bonus), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 3);
                    Enmity.ModifyEnmityOnAll(activator, 8);
                });
        }
    }
}
