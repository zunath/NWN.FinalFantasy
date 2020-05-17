using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class StaticStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder()
                .Create(StatusEffectType.Static)
                .Name("Static")
                .EffectIcon(139)
                .TickAction((activator, target) =>
                {
                    var damage = Random.D4(1);
                    ApplyEffectToObject(DurationType.Instant, EffectDamage(damage, DamageType.Electrical), target);
                });

            return builder.Build();
        }
    }
}
