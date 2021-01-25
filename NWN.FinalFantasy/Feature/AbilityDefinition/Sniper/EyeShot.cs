using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using NWN.FinalFantasy.Core.NWNX;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using NWN.FinalFantasy.Core;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class EyeShotAbilityDefinition : IAbilityListDefinition
    {
        private const string EyeShotVariableName = "EYE_SHOT";
        private const string EyeShotActivatorVariableName = "EYE_SHOT_ACTIVATOR";
        private const float duration = 18.0f;

        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            EyeShot(builder);

            return builder.Build();
        }

        private static void EyeShot(AbilityBuilder builder)
        {
            builder.Create(Feat.EyeShot, PerkType.EyeShot)
                .Name("Eye Shot")
                .HasRecastDelay(RecastGroup.EyeShot, 60.0f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var damageDetails = Damage.GetDamageEventData();
                    if (damageDetails.Total <= 0.0f) return;

                    damageDetails.AdjustAllByPercent(1.5f);
                    ApplyEffectToObject(DurationType.Temporary, EffectAttackDecrease(-5), target, duration);
                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Glow_Red), target, duration);

                    Enmity.ModifyEnmity(activator, target, 4);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.Marksmanship, 2);
                });
        }
    }
}