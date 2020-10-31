using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Creature;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Feat = NWN.FinalFantasy.Core.NWScript.Enum.Feat;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class FlashAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Flash1(builder);
            Flash2(builder);

            return builder.Build();
        }

        private static void Flash1(AbilityBuilder builder)
        {
            builder.Create(Feat.Flash1, PerkType.Flash)
                .Name("Flash I")
                .HasRecastDelay(RecastGroup.Flash, 60f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    var nth = 1;
                    var nearby = GetNearestCreature(CreatureType.IsAlive, 1, target, nth);
                    while (GetIsObjectValid(nearby))
                    {
                        if (GetDistanceBetween(target, nearby) > 5.0f) break;

                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Pdk_Generic_Head_Hit), nearby);
                        StatusEffect.Apply(activator, nearby, StatusEffectType.Blind1, 30f);
                        Enmity.ModifyEnmity(activator, nearby, 35);

                        nth++;
                        nearby = GetNearestCreature(CreatureType.IsAlive, 1, target, nth);
                    }

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Pdk_Generic_Pulse), activator);
                });
        }

        private static void Flash2(AbilityBuilder builder)
        {
            builder.Create(Feat.Flash2, PerkType.Flash)
                .Name("Flash II")
                .HasRecastDelay(RecastGroup.Flash, 60f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    var nth = 1;
                    var nearby = GetNearestCreature(CreatureType.IsAlive, 1, target, nth);
                    while (GetIsObjectValid(nearby))
                    {
                        if (GetDistanceBetween(target, nearby) > 5.0f) break;

                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Pdk_Generic_Head_Hit), nearby);
                        StatusEffect.Apply(activator, nearby, StatusEffectType.Blind2, 45f);
                        Enmity.ModifyEnmity(activator, nearby, 50);

                        nth++;
                        nearby = GetNearestCreature(CreatureType.IsAlive, 1, target, nth);
                    }

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Pdk_Generic_Pulse), activator);
                });
        }
    }
}
