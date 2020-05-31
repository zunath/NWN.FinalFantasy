using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class TeleportAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            TeleportBalamb(builder);

            return builder.Build();
        }

        private static void TeleportBalamb(AbilityBuilder builder)
        {
            builder.Create(Feat.TeleportBalamb, PerkType.TeleportBalamb)
                .Name("Teleport-Balamb")
                .HasRecastDelay(RecastGroup.Teleport, 60f)
                .HasActivationDelay(12f)
                .RequirementMP(40)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasImpactAction((activator, target, level) =>
                {
                    var waypoint = GetWaypointByTag("TELEPORT_BALAMB_LANDING");
                    var location = GetLocation(waypoint);

                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 5.0f))
                    {
                        ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Unsummon), member);

                        AssignCommand(member, () =>
                        {
                            ActionJumpToLocation(location);
                        });
                    }
                });
        }
    }
}
