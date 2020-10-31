using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Creature;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class ProvokeAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Provoke1(builder);
            Provoke2(builder);

            return builder.Build();
        }

        private static void Provoke1(AbilityBuilder builder)
        {
            builder.Create(Feat.Provoke1, PerkType.Provoke)
                .Name("Provoke I")
                .HasRecastDelay(RecastGroup.Provoke1, 30f)
                .RequirementStamina(2)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Destruction), target);
                    Enmity.ModifyEnmity(activator, target, 50);
                });
        }

        private static void Provoke2(AbilityBuilder builder)
        {
            builder.Create(Feat.Provoke2, PerkType.Provoke)
                .Name("Provoke II")
                .HasRecastDelay(RecastGroup.Provoke2, 90f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    var nth = 1;
                    var nearby = GetNearestCreature(CreatureType.IsAlive, 1, target, nth);
                    while (GetIsObjectValid(nearby))
                    {
                        if (GetDistanceBetween(target, nearby) > 5.0f) break;

                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Destruction), nearby);
                        Enmity.ModifyEnmity(activator, nearby, 30);

                        nth++;
                        nearby = GetNearestCreature(CreatureType.IsAlive, 1, target, nth);
                    }

                });
        }
    }
}
