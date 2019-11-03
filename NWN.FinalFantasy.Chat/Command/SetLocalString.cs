using System;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Sets a local string on a target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SetLocalString : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (!GetIsObjectValid(target))
            {
                SendMessageToPC(user, "Target is invalid. Targeting area instead.");
                target = GetArea(user);
            }

            string variableName = Convert.ToString(args[0]);
            string value = string.Empty;

            for (int x = 1; x < args.Length; x++)
            {
                value += " " + args[x];
            }

            value = value.Trim();

            SetLocalString(target, variableName, value);

            SendMessageToPC(user, "Local string set: " + variableName + " = " + value);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length < 1)
            {
                return "Missing arguments. Format should be: /SetLocalString Variable_Name <VALUE>. Example: /SetLocalString MY_VARIABLE My Text";
            }
            
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
