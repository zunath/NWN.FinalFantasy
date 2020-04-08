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
        private const int MPGuiID = 2;
        private const int STMGuiID = 3;

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

            PostString(player, $"HP: {currentHP} / {maxHP}", 50, 20, life: 10.0f, ID: HPGuiID, anchor: ScreenAnchor.Center);
            PostString(player, $"MP: {currentMP} / {maxMP}", 50, 21, life: 10.0f, ID: MPGuiID, anchor: ScreenAnchor.Center);
            PostString(player, $"STM: {currentSTM} / {maxSTM}", 50, 22, life: 10.0f, ID: STMGuiID, anchor: ScreenAnchor.Center);

        }
    }
}
