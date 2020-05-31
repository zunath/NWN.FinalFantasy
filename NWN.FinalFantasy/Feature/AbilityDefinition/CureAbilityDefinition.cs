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
    public class CureAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Cure1(builder);
            Cure2(builder);
            Cure3(builder);

            Curaga1(builder);
            Curaga2(builder);

            return builder.Build();
        }

        private static void Cure1(AbilityBuilder builder)
        {
            builder.Create(Feat.Cure1, PerkType.Cure)
                .Name("Cure I")
                .HasRecastDelay(RecastGroup.Cure1, 5f)
                .HasActivationDelay(2.0f)
                .RequirementMP(2)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var amount = Random.D4(1) + GetAbilityModifier(AbilityType.Wisdom, activator);

                    ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_M), target);

                    Enmity.ModifyEnmityOnAll(activator, amount);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                });
        }

        private static void Cure2(AbilityBuilder builder)
        {
            builder.Create(Feat.Cure2, PerkType.Cure)
                .Name("Cure II")
                .HasRecastDelay(RecastGroup.Cure2, 5f)
                .HasActivationDelay(2.0f)
                .RequirementMP(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var amount = Random.D6(2) + GetAbilityModifier(AbilityType.Wisdom, activator);

                    ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_M), target);

                    Enmity.ModifyEnmityOnAll(activator, amount);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                });
        }

        private static void Cure3(AbilityBuilder builder)
        {
            builder.Create(Feat.Cure3, PerkType.Cure)
                .Name("Cure III")
                .HasRecastDelay(RecastGroup.Cure3, 5f)
                .HasActivationDelay(2.0f)
                .RequirementMP(15)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var amount = Random.D8(3) + GetAbilityModifier(AbilityType.Wisdom, activator);

                    ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_M), target);

                    Enmity.ModifyEnmityOnAll(activator, amount);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                });
        }

        private static void Curaga1(AbilityBuilder builder)
        {
            builder.Create(Feat.Curaga1, PerkType.Curaga)
                .Name("Curaga I")
                .HasRecastDelay(RecastGroup.Curaga1, 12f)
                .HasActivationDelay(4.0f)
                .RequirementMP(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var bonus = GetAbilityModifier(AbilityType.Wisdom, activator);

                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var amount = Random.D6(2) + bonus;

                        ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_M), member);

                        Enmity.ModifyEnmityOnAll(activator, amount);
                    }

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                });
        }

        private static void Curaga2(AbilityBuilder builder)
        {
            builder.Create(Feat.Curaga2, PerkType.Curaga)
                .Name("Curaga II")
                .HasRecastDelay(RecastGroup.Curaga2, 12f)
                .HasActivationDelay(4.0f)
                .RequirementMP(22)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    var bonus = GetAbilityModifier(AbilityType.Wisdom, activator);

                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        var amount = Random.D8(3) + bonus;

                        ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_M), member);

                        Enmity.ModifyEnmityOnAll(activator, amount);
                    }

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 3);
                });
        }
    }
}
