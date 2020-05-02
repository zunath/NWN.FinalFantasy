using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public class AbilityRecastCleanup
    {
        [NWNEventHandler("interval_pc_1s")]
        public static void CleanUpExpiredRecastTimers()
        {
            var player = OBJECT_SELF;
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var now = DateTime.UtcNow;

            foreach (var (group, dateTime) in dbPlayer.RecastTimes)
            {
                if(dateTime > now) continue;

                dbPlayer.RecastTimes.Remove(group);
            }

            DB.Set(playerId, dbPlayer);
        }
    }
}
