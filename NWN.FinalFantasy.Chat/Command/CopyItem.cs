using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Copies the targeted item.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class CopyItem : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (GetObjectType(target) != ObjectType.Item)
            {
                SendMessageToPC(user, "You can only copy items with this command.");
                return;
            }

            CopyItem(target, user, true);
            SendMessageToPC(user, "Item copied successfully.");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => true;
    }
}
