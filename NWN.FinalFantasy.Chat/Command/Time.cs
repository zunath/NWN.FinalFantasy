using System;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Returns the current UTC server time.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Time : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            DateTime now = DateTime.UtcNow;
            string nowText = now.ToString("yyyy-MM-dd hh:mm:ss");

            _.SendMessageToPC(user, "Current Server Date: " + nowText);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}