using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class OneHourAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Invincible(builder);
            Benediction(builder);

            return builder.Build();
        }

        private static void Invincible(AbilityBuilder builder)
        {
            builder.Create(Feat.Invincible, PerkType.Invincible)
                .Name("Invincible")
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Invincible, 30.0f);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 5);
                    Enmity.ModifyEnmityOnAll(activator, 500);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Fnf_Sound_Burst), target);
                });
        }

        private static void Benediction(AbilityBuilder builder)
        {
            builder.Create(Feat.Benediction, PerkType.Benediction)
                .Name("Benediction")
                .DisplaysVisualEffectWhenActivating()
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .UsesActivationType(AbilityActivationType.Casted)
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

    }
}
