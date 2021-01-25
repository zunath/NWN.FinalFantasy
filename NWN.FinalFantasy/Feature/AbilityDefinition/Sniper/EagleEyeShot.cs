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
    public class EagleEyeShotAbilityDefinition: IAbilityListDefinition
    {
        private const string EagleEyeShotVariableName = "EAGLE_EYE_SHOT";
        private const string EagleEyeShotActivatorVariableName = "EAGLE_EYE_SHOT_ACTIVATOR";
        private const float duration = 30.0f;

        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            EagleEyeShot(builder);

            return builder.Build();
        }

        private static void EagleEyeShot(AbilityBuilder builder)
        {
            builder.Create(Feat.EagleEyeShot, PerkType.EagleEyeShot)
                .Name("Eagle Eye Shot")
                .HasRecastDelay(RecastGroup.OneHourAbility, 3600f)
                .RequirementStamina(50)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    SetLocalBool(target, EagleEyeShotVariableName, true);
                    SetLocalObject(target, EagleEyeShotActivatorVariableName, activator);

                    DelayCommand(duration, () => { DeleteLocalBool(activator, EagleEyeShotVariableName); });
                    DelayCommand(duration, () => { DeleteLocalObject(activator, EagleEyeShotActivatorVariableName); });

                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Magical_Sight), activator, duration);
                });
        }

        /// <summary>
        /// When damage is applied, if the target has a Life Steal variable set,
        /// that percent of the damage dealt is restored on the activator.
        /// </summary>
        [NWNEventHandler("on_nwnx_dmg")]
        public static void ApplyEagleEyeShot()
        {
            var target = OBJECT_SELF;
            var damageDetails = Damage.GetDamageEventData();

            if (!GetLocalBool(damageDetails.Damager, EagleEyeShotVariableName)) return;
            if (damageDetails.Total <= 0.0f) return;

            damageDetails.AdjustAllByPercent(5.0f);          
            ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Head_Fire), target);

            Enmity.ModifyEnmity(damageDetails.Damager, target, 4);
            CombatPoint.AddCombatPoint(damageDetails.Damager, target, SkillType.Marksmanship, 2);

            DeleteLocalBool(damageDetails.Damager, EagleEyeShotVariableName);
            DeleteLocalObject(damageDetails.Damager, EagleEyeShotActivatorVariableName);
        }
    }
}