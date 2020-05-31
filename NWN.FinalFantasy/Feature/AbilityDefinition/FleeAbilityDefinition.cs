using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class FleeAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Flee1(builder);
            Flee2(builder);

            return builder.Build();
        }

        private static void Flee1(AbilityBuilder builder)
        {
            builder.Create(Feat.Flee1, PerkType.Flee)
                .Name("Flee I")
                .HasRecastDelay(RecastGroup.Flee, 300f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2.0f)
                .HasImpactAction((activator, target, level) =>
                {
                    var duration = 30f + (GetAbilityModifier(AbilityType.Dexterity, activator) * 2);
                    ApplyEffectToObject(DurationType.Temporary, EffectMovementSpeedIncrease(40), target, duration);

                    Enmity.ModifyEnmityOnAll(activator, 8);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Thievery, 3);
                });
        }

        private static void Flee2(AbilityBuilder builder)
        {
            builder.Create(Feat.Flee2, PerkType.Flee)
                .Name("Flee II")
                .HasRecastDelay(RecastGroup.Flee, 300f)
                .RequirementStamina(25)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2.0f)
                .HasImpactAction((activator, target, level) =>
                {
                    var duration = 30f + (GetAbilityModifier(AbilityType.Dexterity, activator) * 2);
                    ApplyEffectToObject(DurationType.Temporary, EffectMovementSpeedIncrease(80), target, duration);

                    Enmity.ModifyEnmityOnAll(activator, 8);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Thievery, 3);
                });
        }
    }
}
