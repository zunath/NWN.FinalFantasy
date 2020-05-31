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
    public class DiaAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Dia1(builder);
            Dia2(builder);
            Dia3(builder);

            return builder.Build();
        }

        private static void Dia1(AbilityBuilder builder)
        {
            builder.Create(Feat.Dia1, PerkType.Dia)
                .Name("Dia I")
                .HasRecastDelay(RecastGroup.Dia, 3f)
                .HasActivationDelay(1.0f)
                .RequirementMP(2)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Dia1, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Blind_Deaf_M), target);

                    Enmity.ModifyEnmity(activator, target, 6);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.WhiteMagic, 3);
                });
        }

        private static void Dia2(AbilityBuilder builder)
        {
            builder.Create(Feat.Dia2, PerkType.Dia)
                .Name("Dia II")
                .HasRecastDelay(RecastGroup.Dia, 3f)
                .HasActivationDelay(1.0f)
                .RequirementMP(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Dia2, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Blind_Deaf_M), target);

                    Enmity.ModifyEnmity(activator, target, 8);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.WhiteMagic, 3);
                });
        }

        private static void Dia3(AbilityBuilder builder)
        {
            builder.Create(Feat.Dia3, PerkType.Dia)
                .Name("Dia III")
                .HasRecastDelay(RecastGroup.Dia, 3f)
                .HasActivationDelay(1.0f)
                .RequirementMP(6)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Dia3, 60f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Blind_Deaf_M), target);

                    Enmity.ModifyEnmity(activator, target, 10);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.WhiteMagic, 3);
                });
        }
    }
}
