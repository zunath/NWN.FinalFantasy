using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Changes the name of a target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Name : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (GetIsPlayer(target) || GetIsDungeonMaster(target))
            {
                SendMessageToPC(user, "PCs cannot be targeted with this command.");
                return;
            }

            string name = string.Empty;
            foreach (var arg in args)
            {
                name += " " + arg;
            }

            SetName(target, name);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length <= 0)
            {
                return "Please enter a name. Example: /name My Creature";
            }
            
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
