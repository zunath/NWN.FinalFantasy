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
    public class RefreshAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Refresh(builder);

            return builder.Build();
        }

        private static void Refresh(AbilityBuilder builder)
        {
            builder.Create(Feat.Refresh, PerkType.Refresh)
                .Name("Refresh")
                .HasRecastDelay(RecastGroup.Refresh, 18f)
                .RequirementMP(12)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.Refresh, 180f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Head_Mind), target);

                    Enmity.ModifyEnmityOnAll(activator, 6);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 3);
                });
        }
    }
}
