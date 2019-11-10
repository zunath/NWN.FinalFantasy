using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Displays your current coordinates in the area.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Coord : IChatCommand
    {
        /// <summary>
        /// Returns the X and Y position, in tiles, of the user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            Vector position = GetPosition(user);
            int cellX = (int)(position.X / 10);
            int cellY = (int)(position.Y / 10);

            SendMessageToPC(user, $"Current Area Coordinates: ({cellX}, {cellY})");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
