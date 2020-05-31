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
    public class ShockSpikesAbilityDefinition : IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            ShockSpikes1(builder);
            ShockSpikes2(builder);

            return builder.Build();
        }

        private static void ApplyShockSpikes(uint target, int amount, DamageBonus randomAmount)
        {
            const string EffectTag = "SHOCK_SPIKES";
            RemoveEffectByTag(target, EffectTag);

            var newEffect = EffectDamageShield(amount, randomAmount, DamageType.Electrical);
            newEffect = TagEffect(newEffect, EffectTag);
            ApplyEffectToObject(DurationType.Temporary, newEffect, target, 300f);
        }

        private static void ShockSpikes1(AbilityBuilder builder)
        {
            builder.Create(Feat.ShockSpikes1, PerkType.ShockSpikes)
                .Name("Shock Spikes I")
                .HasRecastDelay(RecastGroup.ShockSpikes, 10f)
                .HasActivationDelay(3f)
                .RequirementMP(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Blood_Crt_Yellow), target);
                    ApplyShockSpikes(target, 1, DamageBonus.DAMAGEBONUS_1d4);

                    Enmity.ModifyEnmityOnAll(activator, 6);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 3);
                });
        }

        private static void ShockSpikes2(AbilityBuilder builder)
        {
            builder.Create(Feat.ShockSpikes2, PerkType.ShockSpikes)
                .Name("Shock Spikes II")
                .HasRecastDelay(RecastGroup.ShockSpikes, 10f)
                .HasActivationDelay(3f)
                .RequirementMP(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Blood_Crt_Yellow), target);
                    ApplyShockSpikes(target, 1, DamageBonus.DAMAGEBONUS_2d4);

                    Enmity.ModifyEnmityOnAll(activator, 8);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 3);
                });
        }
    }
}
