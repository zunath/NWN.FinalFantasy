using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public static class AbilityRecastWindow
    {
        private const int MaxNumberOfRecastTimers = 10;

        private const int RecastStartId = 50;
        private const int WindowId = 61;

        [NWNEventHandler("interval_pc_1s")]
        public static void DrawGuiElements()
        {
            DrawRecastComponent(OBJECT_SELF);
        }

        private static void DrawRecastComponent(uint player)
        {
            const int WindowX = 4;
            const int WindowY = 8;
            const int WindowWidth = 25;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var now = DateTime.UtcNow;

            var numberOfRecasts = 0;
            foreach (var (group, dateTime) in dbPlayer.RecastTimes)
            {
                // Max of 10 recasts can be shown in the window.
                if (numberOfRecasts >= MaxNumberOfRecastTimers) break;

                var text = BuildTimerText(group, now, dateTime);
                var centerWindowX = Gui.CenterStringInWindow(text, WindowX, WindowWidth);

                numberOfRecasts++;
                PostString(player, text, centerWindowX+2, WindowY + numberOfRecasts, ScreenAnchor.TopRight, 10.0f, Gui.ColorWhite, Gui.ColorWhite, RecastStartId + numberOfRecasts, Gui.TextName);
            }

            if (numberOfRecasts > 0)
            {
                Gui.DrawWindow(player, WindowId, ScreenAnchor.TopRight, WindowX, WindowY, WindowWidth-2, 1 + numberOfRecasts);
            }
        }

        private static string BuildTimerText(RecastGroup group, DateTime now, DateTime recastTime)
        {
            var recastName = (Ability.GetRecastGroupName(group) + ":").PadRight(14, ' ');
            var delta = recastTime - now;
            var formatTime = delta.ToString(@"hh\:mm\:ss").PadRight(8, ' ');
            return recastName + formatTime;
        }
    }
}
