using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class BurnStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder()
                .Create(StatusEffectType.Burn)
                .Name("Burn")
                .EffectIcon(131)
                .TickAction((source, target) =>
                {
                    AssignCommand(source, () =>
                    {
                        var damage = Random.D6(1);
                        ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Fire), target);
                    });
                });

            return builder.Build();
        }
    }
}
