using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.StatusEffectService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.StatusEffectDefinition
{
    public class RefreshStatusEffectDefinition: IStatusEffectListDefinition
    {
        public Dictionary<StatusEffectType, StatusEffectDetail> BuildStatusEffects()
        {
            var builder = new StatusEffectBuilder();
            Refresh(builder);

            return builder.Build();
        }

        private static void Refresh(StatusEffectBuilder builder)
        {
            builder.Create(StatusEffectType.Refresh)
                .Name("Refresh")
                .EffectIcon(160)
                .TickAction((activator, target) =>
                {
                    if (!GetIsPC(target) || GetIsDM(target)) return;

                    var playerId = GetObjectUUID(target);
                    var dbPlayer = DB.Get<Player>(playerId);

                    Stat.RestoreMP(target, dbPlayer, 3);
                });
        }
    }
}
