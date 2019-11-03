using System;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Sets a local integer on a target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SetLocalInt : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (!GetIsObjectValid(target))
            {
                SendMessageToPC(user, "Target is invalid. Targeting area instead.");
                target = GetArea(user);
            }

            string variableName = args[0];
            int value = Convert.ToInt32(args[1]);

            SetLocalInt(target, variableName, value);

            SendMessageToPC(user, "Local integer set: " + variableName + " = " + value);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length < 2)
            {
                return "Missing arguments. Format should be: /SetLocalInt Variable_Name <VALUE>. Example: /SetLocalInt MY_VARIABLE 69";
            }

            if (!int.TryParse(args[1], out var value))
            {
                return "Invalid value entered. Please try again.";
            }

            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
