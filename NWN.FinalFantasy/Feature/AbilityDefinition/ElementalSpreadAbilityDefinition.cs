using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class ElementalSpreadAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder()
                .Create(Feat.ElementalSpread, PerkType.ElementalSpread)
                .Name("Elemental Spread")
                .HasRecastDelay(RecastGroup.ElementalSpread, 600f)
                .HasActivationDelay(1f)
                .RequirementMP(10)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.ElementalSpread, 60f);

                    Enmity.ModifyEnmityOnAll(activator, 10);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.BlackMagic, 3);
                });

            return builder.Build();
        }
    }
}
