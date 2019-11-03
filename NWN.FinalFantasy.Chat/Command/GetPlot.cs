using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Gets whether an object is marked plot.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class GetPlot : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (GetPlotFlag(target))
            {
                SendMessageToPC(user, "Target is marked plot.");
            }
            else
            {
                SendMessageToPC(user, "Target is NOT marked plot.");
            }
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
