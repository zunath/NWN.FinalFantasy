using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class ElementalSpreadStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder()
                .Create(StatusEffectType.ElementalSpread)
                .Name("Elemental Spread")
                .EffectIcon(133);

            return builder.Build();
        }
    }
}
