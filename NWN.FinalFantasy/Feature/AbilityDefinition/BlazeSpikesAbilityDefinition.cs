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
    public class BlazeSpikesAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            BlazeSpikes1(builder);
            BlazeSpikes2(builder);

            return builder.Build();
        }

        private static void ApplyBlazeSpikes(uint target, int amount, DamageBonus randomAmount)
        {
            const string EffectTag = "BLAZE_SPIKES";
            RemoveEffectByTag(target, EffectTag);
            
            var newEffect = EffectDamageShield(amount, randomAmount, DamageType.Fire);
            newEffect = TagEffect(newEffect, EffectTag);
            ApplyEffectToObject(DurationType.Temporary, newEffect, target, 300f);
        }

        private static void BlazeSpikes1(AbilityBuilder builder)
        {
            builder.Create(Feat.BlazeSpikes1, PerkType.BlazeSpikes)
                .Name("Blaze Spikes I")
                .HasRecastDelay(RecastGroup.BlazeSpikes, 10f)
                .HasActivationDelay(3f)
                .RequirementMP(8)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Blood_Crt_Red), target);
                    ApplyBlazeSpikes(target, 1, DamageBonus.DAMAGEBONUS_1d4);

                    Enmity.ModifyEnmityOnAll(activator, 6);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.BlackMagic, 3);
                });
        }

        private static void BlazeSpikes2(AbilityBuilder builder)
        {
            builder.Create(Feat.BlazeSpikes2, PerkType.BlazeSpikes)
                .Name("Blaze Spikes II")
                .HasRecastDelay(RecastGroup.BlazeSpikes, 10f)
                .HasActivationDelay(3f)
                .RequirementMP(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Blood_Crt_Red), target);
                    ApplyBlazeSpikes(target, 1, DamageBonus.DAMAGEBONUS_2d4);

                    Enmity.ModifyEnmityOnAll(activator, 8);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.BlackMagic, 3);
                });
        }
    }
}
