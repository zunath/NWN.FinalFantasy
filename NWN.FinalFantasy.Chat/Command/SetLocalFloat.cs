using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Sets a local float on a target.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SetLocalFloat : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (!GetIsObjectValid(target))
            {
                SendMessageToPC(user,"Target is invalid. Targeting area instead.");
                target = GetArea(user);
            }

            string variableName = args[0];
            float value = float.Parse(args[1]);

            SetLocalFloat(target, variableName, value);

            SendMessageToPC(user, "Local float set: " + variableName + " = " + value);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length < 2)
            {
                return "Missing arguments. Format should be: /SetLocalFloat Variable_Name <VALUE>. Example: /SetLocalFloat MY_VARIABLE 6.9";
            }
            
            if (!float.TryParse(args[1], out var value))
            {
                return "Invalid value entered. Please try again.";
            }
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
