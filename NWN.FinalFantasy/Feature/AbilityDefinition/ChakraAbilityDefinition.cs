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
    public class ChakraAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Chakra1(builder);
            Chakra2(builder);

            return builder.Build();
        }

        private static void Chakra1(AbilityBuilder builder)
        {
            builder.Create(Feat.Chakra1, PerkType.Chakra)
                .Name("Chakra I")
                .HasRecastDelay(RecastGroup.Chakra, 30f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(10)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = Random.D6(2);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_S), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 2);
                    Enmity.ModifyEnmityOnAll(activator, 6 + hpRecover);
                });
        }

        private static void Chakra2(AbilityBuilder builder)
        {
            builder.Create(Feat.Chakra2, PerkType.Chakra)
                .Name("Chakra II")
                .HasRecastDelay(RecastGroup.Chakra, 30f)
                .HasActivationDelay(2f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(14)
                .HasImpactAction((activator, target, level) =>
                {
                    var hpRecover = Random.D6(3);
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpRecover), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_S), target);
                    StatusEffect.Remove(target, StatusEffectType.Poison1);
                    StatusEffect.Remove(target, StatusEffectType.Poison2);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 3);
                    Enmity.ModifyEnmityOnAll(activator, 8 + hpRecover);
                });
        }
    }
}
