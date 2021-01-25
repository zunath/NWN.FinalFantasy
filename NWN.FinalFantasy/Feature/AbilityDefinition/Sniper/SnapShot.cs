using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Service;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class SnapShotAbilityDefinition: IAbilityListDefinition
    {
        private const string EagleEyeShotVariableName = "EAGLE_EYE_SHOT";
        private const string EagleEyeShotActivatorVariableName = "EAGLE_EYE_SHOT_ACTIVATOR";
        private const float duration = 30.0f;

        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            SnapShot(builder);

            return builder.Build();
        }

        private static void SnapShot(AbilityBuilder builder)
        {
            builder.Create(Feat.SnapShot, PerkType.SnapShot)
                .Name("Snap Shot")
                .HasRecastDelay(RecastGroup.SnapShot, 30.0f)
                .RequirementStamina(5)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    var gun = GetItemInSlot(InventorySlot.RightHand, activator);
                    // TODO: Play gunshot sound
                    // PlaySound("gunshot");
                    
                    /* in-case restrictions need to be added later
                    if (GetBaseItemType(gun) != BaseItem.LightCrossbow)
                    {
                        if (GetIsPC(activator)) SendMessageToPC(activator,"You must have a rifle equipped to use the Snap Shot ability.");
                        return;
                    }
                    */

                    ApplyEffectToObject(DurationType.Instant, EffectDamage(10, DamageType.Piercing), target);
                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Com_Blood_Reg_Red), target);
                    Enmity.ModifyEnmity(activator, target, 4);
                    CombatPoint.AddCombatPoint(activator, target, SkillType.Marksmanship, 2);
                });
        }
    }
}