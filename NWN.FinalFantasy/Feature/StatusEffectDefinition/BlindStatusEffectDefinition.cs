using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class BlindStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder();
            Blind1(builder);
            Blind2(builder);

            return builder.Build();
        }

        private static void Blind1(StatusEffectBuilder builder)
        {
            builder.Create(StatusEffectType.Blind1)
                .Name("Blind I")
                .EffectIcon(146)
                .GrantAction((target, duration) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectMissChance(10), target, duration);
                });
        }

        private static void Blind2(StatusEffectBuilder builder)
        {
            builder.Create(StatusEffectType.Blind2)
                .Name("Blind II")
                .EffectIcon(147)
                .GrantAction((target, duration) =>
                {
                    ApplyEffectToObject(DurationType.Temporary, EffectMissChance(18), target, duration);
                });
        }
    }
}
