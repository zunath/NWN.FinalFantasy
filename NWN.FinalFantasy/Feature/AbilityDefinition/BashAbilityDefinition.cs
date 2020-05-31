using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class BashAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Bash(builder);

            return builder.Build();
        }

        private static void Bash(AbilityBuilder builder)
        {
            builder.Create(Feat.Bash, PerkType.Bash)
                .Name("Bash")
                .HasRecastDelay(RecastGroup.Bash, 12f)
                .RequirementStamina(5)
                .UsesActivationType(AbilityActivationType.Weapon)
                .HasImpactAction((activator, target, level) =>
                {
                    var damage = Random.D4(1);
                    var length = 1f + GetAbilityModifier(AbilityType.Constitution, activator) * 0.5f;

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Confusion_S), target);
                    ApplyEffectToObject(DurationType.Temporary, EffectStunned(), target, length);
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Bludgeoning), target);

                    Enmity.ModifyEnmity(activator, target, 4);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.Chivalry, 2);
                });
        }
    }
}
