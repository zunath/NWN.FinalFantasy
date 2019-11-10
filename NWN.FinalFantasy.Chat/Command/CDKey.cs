using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Displays your public CD key.", CommandPermissionType.DM | CommandPermissionType.Admin | CommandPermissionType.Player)]
    public class CDKey : IChatCommand
    {
        /// <summary>
        /// Displays the public CD key of the user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            string cdKey = GetPCPublicCDKey(user);
            SendMessageToPC(user, "Your public CD Key is: " + cdKey);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
