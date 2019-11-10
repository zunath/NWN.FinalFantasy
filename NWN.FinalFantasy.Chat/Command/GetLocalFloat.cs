using System;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Gets a local float on a target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class GetLocalFloat : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (!GetIsObjectValid(target))
            {
                SendMessageToPC(user, "Target is invalid. Targeting area instead.");
                target = GetArea(user);
            }

            string variableName = Convert.ToString(args[0]);
            float value = GetLocalFloat(target, variableName);

            SendMessageToPC(user, variableName + " = " + value);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length < 1)
            {
                return "Missing arguments. Format should be: /GetLocalFloat Variable_Name. Example: /GetLocalFloat MY_VARIABLE";
            }

            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
