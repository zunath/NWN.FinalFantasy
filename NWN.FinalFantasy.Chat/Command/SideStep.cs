using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Plays a side-step animation.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SideStep : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            _.AssignCommand(user, () => _.ActionPlayAnimation(AnimationFireForget.Dodge_Side));
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
