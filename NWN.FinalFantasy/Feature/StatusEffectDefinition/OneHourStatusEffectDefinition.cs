using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class OneHourStatusEffectDefinition : IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder();
            ElementalSeal(builder);
            Manafont(builder);

            return builder.Build();
        }

        private void ElementalSeal(StatusEffectBuilder builder)
        {
            builder
                .Create(StatusEffectType.ElementalSeal)
                .Name("Elemental Seal")
                .EffectIcon(132);
        }

        private void Manafont(StatusEffectBuilder builder)
        {
            builder
                .Create(StatusEffectType.Manafont)
                .Name("Manafont")
                .EffectIcon(156);
        }
    }
}