using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Displays your current position in the area.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Pos: IChatCommand
    {
        /// <summary>
        /// Returns the current position of user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            var position = GetPosition(user);
            SendMessageToPC(user, $"Current Position: ({position.X}, {position.Y}, {position.Z})");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
