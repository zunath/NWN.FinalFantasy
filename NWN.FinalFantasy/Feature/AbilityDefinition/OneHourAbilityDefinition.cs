using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item.Property;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using DamageType = NWN.FinalFantasy.Core.NWScript.Enum.DamageType;
using Feat = NWN.FinalFantasy.Core.NWScript.Enum.Feat;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class OneHourAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Invincible(builder);
            HundredFists(builder);
            Benediction(builder);
            ElementalSeal(builder);
            PerfectDodge(builder);

            return builder.Build();
        }

        private static void Invincible(AbilityBuilder builder)
        {
            builder.Create(Feat.Invincible, PerkType.Invincible)
                .Name("Invincible")
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(50)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Invincible, 30.0f);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 5);
                    Enmity.ModifyEnmityOnAll(activator, 500);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Fnf_Sound_Burst), target);
                });
        }

        private static void HundredFists(AbilityBuilder builder)
        {
            builder.Create(Feat.HundredFists, PerkType.HundredFists)
                .Name("Hundred Fists")
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(50)
                .HasImpactAction((activator, target, level) =>
                {
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chi, 5);
                    Enmity.ModifyEnmityOnAll(activator, 250);

                    var effect = EffectLinkEffects(EffectHaste(), EffectModifyAttacks(5));
                    effect = TagEffect(effect, "HUNDRED_FISTS");

                    ApplyEffectToObject(DurationType.Temporary, effect, target, 30.0f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Knock), target);
                });
        }

        private static void Benediction(AbilityBuilder builder)
        {
            builder.Create(Feat.Benediction, PerkType.Benediction)
                .Name("Benediction")
                .DisplaysVisualEffectWhenActivating()
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementMP(50)
                .HasImpactAction((activator, target, level) =>
                {
                    var members = Party.GetAllPartyMembersWithinRange(activator, 15.0f);

                    foreach (var member in members)
                    {
                        var maxHP = GetMaxHitPoints(member);
                        ApplyEffectToObject(DurationType.Instant, EffectHeal(maxHP), member);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Healing_X), member);
                    }

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 5);
                    Enmity.ModifyEnmityOnAll(activator, 300 + members.Count * 50);
                });
        }

        private static void ElementalSeal(AbilityBuilder builder)
        {
            builder.Create(Feat.ElementalSeal, PerkType.ElementalSeal)
                .Name("Elemental Seal")
                .DisplaysVisualEffectWhenActivating()
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementMP(50)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.ElementalSeal, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Fnf_Howl_Mind), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.BlackMagic, 5);
                    Enmity.ModifyEnmityOnAll(activator, 300);
                });
        }

        private static void PerfectDodge(AbilityBuilder builder)
        {
            builder.Create(Feat.PerfectDodge, PerkType.PerfectDodge)
                .Name("Perfect Dodge")
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .RequirementStamina(50)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectACIncrease(20), target, 30f);
                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Pixiedust), target, 30f);
                });
        }

    }
}
