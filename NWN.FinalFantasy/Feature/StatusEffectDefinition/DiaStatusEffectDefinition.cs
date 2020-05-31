using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class DiaStatusEffectDefinition : IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder();
            Dia1(builder);
            Dia2(builder);
            Dia3(builder);

            return builder.Build();
        }

        private static void Dia1(StatusEffectBuilder builder)
        {
            builder.Create(StatusEffectType.Dia1)
                .Name("Dia I")
                .EffectIcon(153)
                .GrantAction((target, duration) =>
                {
                    if (StatusEffect.HasStatusEffect(target, StatusEffectType.Dia2) ||
                        StatusEffect.HasStatusEffect(target, StatusEffectType.Dia3))
                    {
                        SendMessageToPC(target, "A more powerful version of Dia is already in effect.");
                        StatusEffect.Remove(target, StatusEffectType.Dia1);
                    }
                })
                .TickAction((activator, target) =>
                {
                    var damage = Random.D2(1);

                    AssignCommand(activator, () =>
                    {
                        ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Positive), target);
                    });
                });
        }

        private static void Dia2(StatusEffectBuilder builder)
        {
            builder.Create(StatusEffectType.Dia2)
                .Name("Dia II")
                .EffectIcon(154)
                .GrantAction((target, duration) =>
                {
                    if (StatusEffect.HasStatusEffect(target, StatusEffectType.Dia3))
                    {
                        SendMessageToPC(target, "A more powerful version of Dia is already in effect.");
                        StatusEffect.Remove(target, StatusEffectType.Dia2);
                        return;
                    }

                    StatusEffect.Remove(target, StatusEffectType.Dia1);
                })
                .TickAction((activator, target) =>
                {
                    var damage = Random.D4(1);

                    AssignCommand(activator, () =>
                    {
                        ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Positive), target);
                    });
                });
        }

        private static void Dia3(StatusEffectBuilder builder)
        {
            builder.Create(StatusEffectType.Dia3)
                .Name("Dia III")
                .EffectIcon(155)
                .GrantAction((target, duration) =>
                {
                    StatusEffect.Remove(target, StatusEffectType.Dia1);
                    StatusEffect.Remove(target, StatusEffectType.Dia2);
                })
                .TickAction((activator, target) =>
                {
                    var damage = Random.D6(1);

                    AssignCommand(activator, () =>
                    {
                        ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Positive), target);
                    });
                });
        }
    }
}
