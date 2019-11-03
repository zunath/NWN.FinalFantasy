using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Teleports you to a waypoint with a specified tag.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class TpWp : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            string tag = args[0];
            NWGameObject wp = GetWaypointByTag(tag);

            if (!GetIsObjectValid(wp))
            {
                SendMessageToPC(user, "Invalid waypoint tag. Did you enter the right tag?");
                return;
            }

            AssignCommand(user, () => ActionJumpToLocation(GetLocation(wp)));
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length < 1)
            {
                return "You must specify a waypoint tag. Example: /tpwp MY_WAYPOINT_TAG";
            }

            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
