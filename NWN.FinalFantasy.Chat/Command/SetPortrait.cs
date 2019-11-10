using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Sets portrait of the target player using the string specified. (Remember to add po_ to the portrait)", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SetPortrait : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (!GetIsObjectValid(target) || GetObjectType(target) != ObjectType.Creature)
            {
                SendMessageToPC(user, "Only creatures may be targeted with this command.");
                return;
            }

            SetPortraitResRef(target, args[0]);
            FloatingTextStringOnCreature(target, "Your portrait has been changed.");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length <= 0)
            {
                return "Please enter the name of the portrait and try again. Example: /SetPortrait po_myportrait";
            }

            if (args[0].Length > 16)
            {
                return "The portrait you entered is too long. Portrait names should be between 1 and 16 characters.";
            }


            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
