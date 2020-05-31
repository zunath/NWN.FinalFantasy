using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class DeliberateStabStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder()
                .Create(StatusEffectType.DeliberateStab)
                .EffectIcon(161);

            return builder.Build();
        }
    }
}
