using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public class GuiElements
    {
        private const int HPGuiID = 1;
        private const int HPBackgroundGuiID = 2;
        private const int HPTextGuiID = 3;

        private const int MPGuiID = 4;
        private const int MPBackgroundGuiID = 5;
        private const int MPTextGuiID = 6;

        private const int STMGuiID = 7;
        private const int STMBackgroundGuiID = 8;
        private const int STMTextGuiID = 9;

        private const int WindowId = 10;

        /// <summary>
        /// On module heartbeat, draws all GUI elements on every player's screen.
        /// </summary>
        [NWNEventHandler("mod_heartbeat")]
        public static void DrawGuiElements()
        {
            for (var player = GetFirstPC(); GetIsObjectValid(player); player = GetNextPC())
            {
                DrawStatusComponent(player);
            }
        }

        /// <summary>
        /// Draws the HP, MP, and STM status information on the player's screen.
        /// </summary>
        /// <param name="player">The player to draw the component for.</param>
        private static void DrawStatusComponent(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId) ?? new Player();

            var currentHP = GetCurrentHitPoints(player);
            var maxHP = GetMaxHitPoints(player);
            var currentMP = dbPlayer.MP;
            var maxMP = dbPlayer.MaxMP;
            var currentSTM = dbPlayer.Stamina;
            var maxSTM = dbPlayer.MaxStamina;


            var backgroundBar = BuildBar(1, 1, 22);
            var hpBar = BuildBar(currentHP, maxHP, 22);
            var mpBar = BuildBar(currentMP, maxMP, 22);
            var stmBar = BuildBar(currentSTM, maxSTM, 22);

            // Draw order is backwards. The top-most layer needs to be drawn first.
            var centerWindowX = Gui.CenterStringInWindow(backgroundBar, 16, 25);

            // Draw the text
            PostString(player, $"HP: {currentHP} / {maxHP}", centerWindowX + 9, 1, ScreenAnchor.TopRight, 10.0f, Gui.ColorWhite, Gui.ColorWhite, HPTextGuiID, Gui.TextName);
            PostString(player, $"MP: {currentMP} / {maxMP}", centerWindowX + 9, 2, ScreenAnchor.TopRight, 10.0f, Gui.ColorWhite, Gui.ColorWhite, MPTextGuiID, Gui.TextName);
            PostString(player, $"STM: {currentSTM} / {maxSTM}", centerWindowX + 9, 3, ScreenAnchor.TopRight, 10.0f, Gui.ColorWhite, Gui.ColorWhite, STMTextGuiID, Gui.TextName);

            // Draw the bars
            PostString(player, hpBar, centerWindowX + 2, 1, ScreenAnchor.TopRight, 10.0f, Gui.ColorHealthBar, Gui.ColorHealthBar, HPGuiID, Gui.FontName);
            PostString(player, mpBar, centerWindowX + 2, 2, ScreenAnchor.TopRight, 10.0f, Gui.ColorManaBar, Gui.ColorManaBar, MPGuiID, Gui.FontName);
            PostString(player, stmBar, centerWindowX + 2, 3, ScreenAnchor.TopRight, 10.0f, Gui.ColorStaminaBar, Gui.ColorStaminaBar, STMGuiID, Gui.FontName);

            // Draw the backgrounds
            PostString(player, backgroundBar, centerWindowX + 2, 1, ScreenAnchor.TopRight, 10.0f, Gui.ColorBlack, Gui.ColorBlack, HPBackgroundGuiID, Gui.FontName);
            PostString(player, backgroundBar, centerWindowX + 2, 2, ScreenAnchor.TopRight, 10.0f, Gui.ColorBlack, Gui.ColorBlack, MPBackgroundGuiID, Gui.FontName);
            PostString(player, backgroundBar, centerWindowX + 2, 3, ScreenAnchor.TopRight, 10.0f, Gui.ColorBlack, Gui.ColorBlack, STMBackgroundGuiID, Gui.FontName);

            Gui.DrawWindow(player, WindowId, ScreenAnchor.TopRight, 16, 0, 23, 3);

        }

        /// <summary>
        /// Builds a bar for display with the PostString call.
        /// </summary>
        /// <param name="current">The current value to display.</param>
        /// <param name="maximum">The maximum value to display.</param>
        /// <param name="width"></param>
        /// <returns></returns>
        private static string BuildBar(int current, int maximum, int width)
        {
            if (current <= 0) return string.Empty;

            var unitsPerWidth = (maximum / (float)width);
            var currentNumber = Math.Ceiling(current / unitsPerWidth);
            string bar = string.Empty;

            for(var x = 0; x < currentNumber; x++)
            {
                bar += Gui.BlankWhite;
            }

            return bar;
        }
    }
}
