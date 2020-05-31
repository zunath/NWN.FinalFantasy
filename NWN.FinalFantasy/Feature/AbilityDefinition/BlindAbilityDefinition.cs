using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class BlindAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Blind1(builder);
            Blind2(builder);

            return builder.Build();
        }

        private static void Blind1(AbilityBuilder builder)
        {
            builder.Create(Feat.Blind1, PerkType.Blind)
                .Name("Blind I")
                .HasRecastDelay(RecastGroup.Blind, 12f)
                .RequirementMP(6)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2f)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Blind1, 15f);

                    Enmity.ModifyEnmity(activator, target, 4);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 3);
                });
        }

        private static void Blind2(AbilityBuilder builder)
        {
            builder.Create(Feat.Blind2, PerkType.Blind)
                .Name("Blind II")
                .HasRecastDelay(RecastGroup.Blind, 12f)
                .RequirementMP(10)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2f)
                .HasImpactAction((activator, target, level) =>
                {
                    // Intentional to use Blind I again.
                    StatusEffect.Apply(activator, target, StatusEffectType.Blind1, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Dazed_S), target);

                    Enmity.ModifyEnmity(activator, target, 6);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 3);
                });
        }
    }
}
