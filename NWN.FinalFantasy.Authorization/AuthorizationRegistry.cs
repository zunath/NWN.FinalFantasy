using System.Linq;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Data.Repository;

namespace NWN.FinalFantasy.Authorization
{
    public static class AuthorizationRegistry
    {
        /// <summary>
        /// Retrieves the authorization level of a given player.
        /// </summary>
        /// <param name="player">The player whose authorization level we're checking</param>
        /// <returns>The authorization level (player, DM, or admin)</returns>
        public static AuthorizationLevel GetAuthorizationLevel(NWGameObject player)
        {
            var dmList = DMRepo.Get();
            var cdKey = _.GetPCPublicCDKey(player);

            var existing = dmList.Entities.FirstOrDefault(x => x.CDKey == cdKey);
            if (existing == null)
                return AuthorizationLevel.Player;

            return existing.Authorization;
        }
    }
}
