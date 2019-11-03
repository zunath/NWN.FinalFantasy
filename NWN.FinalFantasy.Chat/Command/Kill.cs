using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Kills your target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Kill : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            var amount = _.GetMaxHitPoints(target) + 11;
            var damage = _.EffectDamage(amount);
            _.ApplyEffectToObject(DurationType.Instant, damage, target);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
