using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    public static class Achievement
    {
        private static Dictionary<AchievementType, AchievementAttribute> _allAchievements = new Dictionary<AchievementType, AchievementAttribute>();
        private static Dictionary<AchievementType, AchievementAttribute> _activeAchievements = new Dictionary<AchievementType, AchievementAttribute>();

        /// <summary>
        /// When the module loads, read all achievement types and store them into the cache.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void LoadAchievements()
        {
            var achievementTypes = Enum.GetValues(typeof(AchievementType)).Cast<AchievementType>();
            foreach (var achievement in achievementTypes)
            {
                var achievementDetail = achievement.GetAttribute<AchievementType, AchievementAttribute>();
                _allAchievements[achievement] = achievementDetail;

                if (achievementDetail.IsActive)
                {
                    _activeAchievements[achievement] = achievementDetail;
                }
            }
        }

        /// <summary>
        /// Gives an achievement to a player's account (by CD key).
        /// If the player already has this achievement, nothing will happen.
        /// </summary>
        /// <param name="player">The player to give the achievement to.</param>
        /// <param name="achievement">The achievement to grant.</param>
        public static void GiveAchievement(uint player, AchievementType achievement)
        {
            if (!GetIsPC(player) || GetIsDM(player)) return;

            var cdKey = GetPCPublicCDKey(player);
            var account = DB.Get<Account>(cdKey) ?? new Account();
            if (account.Achievements.ContainsKey(achievement)) return;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var now = DateTime.UtcNow;
            account.Achievements[achievement] = now;
            DB.Set(cdKey, account);

            // Player turned off achievement notifications. Nothing left to do here.
            if (!dbPlayer.Settings.DisplayAchievementNotification) return;

            DisplayAchievementNotificationWindow();
        }

        private static void DisplayAchievementNotificationWindow()
        {

        }
    }
}
