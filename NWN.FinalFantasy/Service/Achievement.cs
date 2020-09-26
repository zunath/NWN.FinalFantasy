using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    public static class Achievement
    {
        private static Gui.IdReservation _idReservation;
        private static readonly Dictionary<AchievementType, AchievementAttribute> _activeAchievements = new Dictionary<AchievementType, AchievementAttribute>();

        [NWNEventHandler("mod_load")]
        public static void ReserveGuiIds()
        {
            _idReservation = Gui.ReserveIds(nameof(Achievement), 6);
        }

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
        /// <param name="achievementType">The achievement to grant.</param>
        public static void GiveAchievement(uint player, AchievementType achievementType)
        {
            if (!GetIsPC(player) || GetIsDM(player)) return;

            var cdKey = GetPCPublicCDKey(player);
            var account = DB.Get<Account>(cdKey) ?? new Account();
            if (account.Achievements.ContainsKey(achievementType)) return;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var now = DateTime.UtcNow;
            account.Achievements[achievementType] = now;
            DB.Set(cdKey, account);

            // Player turned off achievement notifications. Nothing left to do here.
            if (!dbPlayer.Settings.DisplayAchievementNotification) return;

            var achievement = _activeAchievements[achievementType];
            DisplayAchievementNotificationWindow(player, achievement.Name);
            Core.NWNX.Player.PlaySound(player, "gui_prompt", OBJECT_INVALID);
        }

        /// <summary>
        /// Displays the achievement notification window with the achievement's name and description.
        /// </summary>
        private static void DisplayAchievementNotificationWindow(uint player, string name)
        {
            const int WindowX = 2;
            const int WindowY = 2;
            const int WindowWidth = 26;

            var centerWindowX = Gui.CenterStringInWindow(name, WindowX, WindowWidth);
            PostString(player, "Achievement Unlocked", centerWindowX-1, WindowY+1, ScreenAnchor.TopLeft, 10.0f, Gui.ColorWhite, Gui.ColorYellow, _idReservation.StartId,Gui.TextName);
            PostString(player, " " + name, centerWindowX-1, WindowY+3, ScreenAnchor.TopLeft, 10.0f, Gui.ColorWhite, Gui.ColorYellow, _idReservation.StartId + 1, Gui.TextName);
            Gui.DrawWindow(player, _idReservation.StartId + 2, ScreenAnchor.TopLeft, WindowX, WindowY, WindowWidth, 4);
        }

        /// <summary>
        /// Retrieves all of the active achievements.
        /// </summary>
        /// <returns>A dictionary containing all of the active achievements.</returns>
        public static Dictionary<AchievementType, AchievementAttribute> GetActiveAchievements()
        {
            return _activeAchievements.ToDictionary(x => x.Key, y => y.Value);
        }
    }
}
