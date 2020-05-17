using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class SleepStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            const string SleepEffectTag = "STATUS_EFFECT_SLEEP";

            var builder = new StatusEffectBuilder()
                .Create(StatusEffectType.Sleep)
                .Name("Sleep")
                .EffectIcon(134)
                .GrantAction((target, duration) =>
                {
                    var effect = EffectStunned();
                    effect = EffectLinkEffects(effect, EffectVisualEffect(VisualEffect.Vfx_Dur_Iounstone_Blue));
                    effect = EffectLinkEffects(effect, EffectVisualEffect(VisualEffect.Vfx_Imp_Sleep));
                    effect = TagEffect(effect, SleepEffectTag);
                    ApplyEffectToObject(DurationType.Temporary, effect, target, duration);
                })
                .RemoveAction(target =>
                {
                    RemoveEffectByTag(target, SleepEffectTag);
                });

            return builder.Build();
        }

        /// <summary>
        /// When a PC or creature is damaged, remove any Sleep status effects which may be present.
        /// </summary>
        [NWNEventHandler("pc_damaged")]
        [NWNEventHandler("crea_damaged")]
        public static void DamageCreature()
        {
            if (!StatusEffect.HasStatusEffect(OBJECT_SELF, StatusEffectType.Sleep)) return;
            StatusEffect.Remove(OBJECT_SELF, StatusEffectType.Sleep);
        }

    }
}
