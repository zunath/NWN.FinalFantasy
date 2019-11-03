namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Sets the world time to 8 AM.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Day: IChatCommand
    {
        /// <summary>
        /// Sets the time to 8 AM
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            _.SetTime(8, 0, 0, 0);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
