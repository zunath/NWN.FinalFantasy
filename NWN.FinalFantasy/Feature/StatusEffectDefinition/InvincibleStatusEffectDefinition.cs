using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class InvincibleStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder()
                .Create(StatusEffectType.Invincible)
                .Name("Invincible")
                .EffectIcon(130)
                .GrantAction((target, duration) =>
                {
                    SetPlotFlag(target, true);
                })
                .RemoveAction((target) =>
                {
                    SetPlotFlag(target, false);
                });

            return builder.Build();
        }
    }
}
