using NWN.FinalFantasy.Core.Dialog;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Opens the rest menu.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Rest : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            Conversation.Start(user, user, "RestMenu");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
