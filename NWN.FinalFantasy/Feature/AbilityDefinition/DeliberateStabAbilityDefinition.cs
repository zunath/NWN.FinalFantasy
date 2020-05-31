using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class DeliberateStabAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            DeliberateStab(builder);

            return builder.Build();
        }

        private static void DeliberateStab(AbilityBuilder builder)
        {
            builder.Create(Feat.DeliberateStab, PerkType.DeliberateStab)
                .Name("Deliberate Stab")
                .HasRecastDelay(RecastGroup.DeliberateStab, 300f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(2f)
                .HasImpactAction((activator, target, level) =>
                {
                    StatusEffect.Apply(activator, target, StatusEffectType.DeliberateStab, 30f);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Divine_Strike_Holy), activator);

                    Enmity.ModifyEnmityOnAll(activator, 3);
                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 3);
                });
        }
    }
}
