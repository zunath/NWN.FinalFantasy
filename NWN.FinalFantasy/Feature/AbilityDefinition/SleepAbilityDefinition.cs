using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class SleepAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder()
                .Create(Feat.Sleep, PerkType.Sleep)
                .Name("Sleep")
                .HasRecastDelay(RecastGroup.Sleep, 12f)
                .HasActivationDelay(2f)
                .RequirementMP(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var resistance = Resistance.GetResistance(target, ResistanceType.Sleep);
                    var baseDuration = Random.NextFloat(15.0f, 30.0f);
                    var duration = baseDuration * resistance;

                    StatusEffect.Apply(activator, target, StatusEffectType.Sleep, duration);
                    Resistance.ModifyResistance(target, ResistanceType.Sleep, -0.25f);
                });

            return builder.Build();
        }
    }
}
