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
    public class SpikedDefenseAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder()
                .Create(Feat.SpikedDefense, PerkType.SpikedDefense)
                .HasRecastDelay(RecastGroup.SpikedDefense, 120f)
                .RequirementStamina(10)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    const float Duration = 60f;

                    ApplyEffectToObject(DurationType.Temporary, EffectVisualEffect(VisualEffect.Vfx_Dur_Aura_Pulse_Red_Orange), target, Duration);
                    ApplyEffectToObject(DurationType.Temporary, EffectDamageShield(2, DamageBonus.DAMAGEBONUS_1d4, DamageType.Piercing), target);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.Chivalry, 3);
                    Enmity.ModifyEnmityOnAll(activator, 10);
                });

            return builder.Build();
        }
    }
}
