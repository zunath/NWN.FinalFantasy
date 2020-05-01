using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Entity;

namespace NWN.FinalFantasy.Service.AbilityService
{
    /// <summary>
    /// Adds a stamina requirement to activate a perk.
    /// </summary>
    public class AbilityStaminaRequirement : IAbilityActivationRequirement
    {
        private readonly int _requiredSTM;

        public AbilityStaminaRequirement(int requiredSTM)
        {
            _requiredSTM = requiredSTM;
        }

        public string CheckRequirements(uint player)
        {
            // NPCs and DMs are assumed to be able to activate.
            if (!NWScript.GetIsPC(player) || NWScript.GetIsDM(player)) return string.Empty;

            var playerId = NWScript.GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            if (dbPlayer.Stamina > _requiredSTM) return string.Empty;
            return $"Not enough stamina. (Required: {_requiredSTM})";
        }

        public void AfterActivationAction(uint player)
        {
            var playerId = NWScript.GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.ReduceStamina(dbPlayer, _requiredSTM);

            DB.Set(playerId, dbPlayer);
        }
    }
}