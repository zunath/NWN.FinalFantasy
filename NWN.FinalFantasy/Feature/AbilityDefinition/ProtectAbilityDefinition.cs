using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class ProtectAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Protect(builder);
            Protectra(builder);

            return builder.Build();
        }

        private static void Protect(AbilityBuilder builder)
        {
            builder.Create(Feat.Protect, PerkType.Protect)
                .Name("Protect")
                .HasRecastDelay(RecastGroup.Protect, 4f)
                .HasActivationDelay(2.0f)
                .RequirementMP(4)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectDamageReduction(4, DamagePower.Normal), target, 1800f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Ac_Bonus), target);

                    Enmity.ModifyEnmityOnAll(activator, 4);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 2);
                });
        }

        private static void Protectra(AbilityBuilder builder)
        {
            builder.Create(Feat.Protectra, PerkType.Protectra)
                .Name("Protectra")
                .HasRecastDelay(RecastGroup.Protectra, 10f)
                .HasActivationDelay(3.0f)
                .RequirementMP(10)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        ApplyEffectToObject(DurationType.Temporary, EffectDamageReduction(4, DamagePower.Normal), member, 1800f);
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Ac_Bonus), member);

                        Enmity.ModifyEnmityOnAll(activator, 2);
                    }

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.WhiteMagic, 4);
                });
        }
    }
}
