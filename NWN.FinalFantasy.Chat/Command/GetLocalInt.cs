using System;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Gets a local integer on a target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class GetLocalInt : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (!GetIsObjectValid(target))
            {
                SendMessageToPC(user, "Target is invalid. Targeting area instead.");
                target = GetArea(user);
            }

            string variableName = Convert.ToString(args[0]);
            int value = GetLocalInt(target, variableName);

           SendMessageToPC(user, variableName + " = " + value);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length < 1)
            {
                return "Missing arguments. Format should be: /GetLocalInt Variable_Name. Example: /GetLocalInt MY_VARIABLE";
            }
            
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
