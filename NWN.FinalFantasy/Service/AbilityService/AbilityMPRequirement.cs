using NWN.FinalFantasy.Entity;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service.AbilityService
{
    /// <summary>
    /// Adds an MP requirement to activate a perk.
    /// </summary>
    public class PerkMPRequirement : IAbilityActivationRequirement
    {
        private readonly int _requiredMP;

        public PerkMPRequirement(int requiredMP)
        {
            _requiredMP = requiredMP;
        }

        public string CheckRequirements(uint player)
        {
            // NPCs and DMs are assumed to be able to activate.
            if (!GetIsPC(player) || GetIsDM(player)) return string.Empty;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            if (dbPlayer.MP > _requiredMP) return string.Empty;

            return $"Not enough MP. (Required: {_requiredMP})";
        }

        public void AfterActivationAction(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.ReduceMP(dbPlayer, _requiredMP);

            DB.Set(playerId, dbPlayer);
        }
    }
}
