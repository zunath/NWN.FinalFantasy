using NWN.FinalFantasy.Core.Utility;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Spawns an item of a specific quantity on your character. Example: /spawnitem my_item 3", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SpawnItem: IChatCommand
    {
        /// <summary>
        /// Spawns an item by resref in the user's inventory.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            string resref = args[0];
            int quantity = 1;

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out quantity))
                {
                    return;
                }
            }

            NWGameObject item = (CreateItemOnObject(resref, user, quantity));

            if (!GetIsObjectValid(item))
            {
                SendMessageToPC(user, ColorToken.Red("Item not found! Did you enter the correct ResRef?"));
                return;
            }

            SetIdentified(item, true);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length <= 0)
            {
                return ColorToken.Red("Please specify a resref and optionally a quantity. Example: /" + nameof(SpawnItem) + " my_resref 20");
            }

            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
