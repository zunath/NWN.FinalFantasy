using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Death
{
    public class OnPlayerDeath: IScript
    {
        /// <summary>
        /// Handles resetting a player's faction reputation toward Commoner, Merchant, and Defender
        /// standard factions back to normal, as well as any personal reputation the player may have
        /// obtained toward this killer's faction.
        /// Pops up the respawn panel and writes an audit log as well.
        /// </summary>
        public void Main()
        {
            NWGameObject player = GetLastPlayerDied();
            NWGameObject hostile = GetLastHostileActor(player);

            SetStandardFactionReputation(StandardFaction.Commoner, 100, player);
            SetStandardFactionReputation(StandardFaction.Merchant, 100, player);
            SetStandardFactionReputation(StandardFaction.Defender, 100, player);

            var factionMember = GetFirstFactionMember(hostile, false);
            while (GetIsObjectValid(factionMember))
            {
                ClearPersonalReputation(player, factionMember);
                factionMember = GetNextFactionMember(hostile, false);
            }

            const string RespawnMessage = "You have died. You can wait for another player to revive you or respawn to go to your home point.";
            PopUpDeathGUIPanel(player, true, true, 0, RespawnMessage);

            WriteAudit(player);
        }

        /// <summary>
        /// Write an audit entry with details of this death.
        /// </summary>
        /// <param name="player">The player who died</param>
        private static void WriteAudit(NWGameObject player)
        {
            var name = GetName(player);
            var area = GetArea(player);
            var areaName = GetName(area);
            var areaTag = GetTag(area);
            var areaResref = GetResRef(area);
            NWGameObject hostile = GetLastHostileActor(player);
            var hostileName = GetName(hostile);

            var log = $"DEATH: {name} - {areaName} - {areaTag} - {areaResref} Killed by: {hostileName}";
        }
    }
}
