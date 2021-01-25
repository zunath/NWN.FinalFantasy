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
    public class ArmShotAbilityDefinition : IAbilityListDefinition
    {
        private const float duration = 6.0f;

        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            ArmShot(builder);

            return builder.Build();
        }

        private static void ArmShot(AbilityBuilder builder)
        {
            builder.Create(Feat.ArmShot, PerkType.ArmShot)
                .Name("Arm Shot")
                .HasRecastDelay(RecastGroup.ArmShot, 12.0f)
                .RequirementStamina(10)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectAttackDecrease(-5), target, duration);
                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Glow_Light_Red), target, duration);

                    Enmity.ModifyEnmity(activator, target, 4);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.Marksmanship, 2);
                });
        }
    }
}