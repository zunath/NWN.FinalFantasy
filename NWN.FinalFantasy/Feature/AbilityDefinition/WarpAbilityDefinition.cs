using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class WarpAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Warp1(builder);
            Warp2(builder);

            return builder.Build();
        }

        private static void SendToHomePoint(uint player)
        {
            if (!GetIsPC(player) || GetIsDM(player)) return;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            var position = Vector(dbPlayer.RespawnLocationX, dbPlayer.RespawnLocationY, dbPlayer.RespawnLocationZ);
            var area = Cache.GetAreaByResref(dbPlayer.RespawnAreaResref);
            var location = Location(area, position, dbPlayer.RespawnLocationOrientation);

            AssignCommand(player, () =>
            {
                ClearAllActions();
                ActionJumpToLocation(location);
            });
        }

        private static void Warp1(AbilityBuilder builder)
        {
            builder.Create(Feat.Warp1, PerkType.Warp)
                .Name("Warp I")
                .HasRecastDelay(RecastGroup.Warp, 30f)
                .HasActivationDelay(12.0f)
                .RequirementMP(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    SendToHomePoint(target);
                });
        }

        private static void Warp2(AbilityBuilder builder)
        {
            builder.Create(Feat.Warp2, PerkType.Warp)
                .Name("Warp II")
                .HasRecastDelay(RecastGroup.Warp, 30f)
                .HasActivationDelay(12.0f)
                .RequirementMP(40)
                .UsesActivationType(AbilityActivationType.Casted)
                .DisplaysVisualEffectWhenActivating()
                .HasImpactAction((activator, target, level) =>
                {
                    foreach (var member in Party.GetAllPartyMembersWithinRange(activator, 10.0f))
                    {
                        SendToHomePoint(member);
                    }
                });
        }
    }
}
