using System;
using System.Linq;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Authorization
{
    public static class AuthorizationRegistry
    {
        private const string RegistrationKey = "Authorization.DMList.Key";

        /// <summary>
        /// Retrieves the authorization level of a given player.
        /// </summary>
        /// <param name="player">The player whose authorization level we're checking</param>
        /// <returns>The authorization level (player, DM, or admin)</returns>
        public static AuthorizationLevel GetAuthorizationLevel(NWGameObject player)
        {
            var dmList = GetDMList();
            var cdKey = _.GetPCPublicCDKey(player);

            var existing = dmList.Entities.FirstOrDefault(x => x.CDKey == cdKey);
            if (existing == null)
                return AuthorizationLevel.Player;

            return existing.Authorization;
        }

        /// <summary>
        /// Retrieves the DM list or creates it if it doesn't yet exist.
        /// </summary>
        /// <returns>A DM list</returns>
        private static DMList GetDMList()
        {
            // DM list doesn't exist in Redis. Create a new entry and push it up.
            if (!NWNXRedis.Exists(RegistrationKey))
            {
                var dmList = new DMList();
                
                NWNXRedis.Set(RegistrationKey, dmList.ID.ToString());
                DB.Set(dmList);
            }

            var key = new Guid(NWNXRedis.Get(RegistrationKey));
            return DB.Get<DMList>(key);
        }
    }
}
