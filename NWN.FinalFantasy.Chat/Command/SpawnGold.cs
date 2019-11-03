using NWN.FinalFantasy.Core.Utility;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Spawns gold of a specific quantity on your character. Example: /spawngold 33", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class SpawnGold : IChatCommand
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
            int quantity = 1;

            if (args.Length >= 1)
            {
                if (!int.TryParse(args[0], out quantity))
                {
                    return;
                }
            }

            _.GiveGoldToCreature(user, quantity);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length <= 0)
            {
                return ColorToken.Red("Please specify a quantity. Example: /" + nameof(SpawnGold) + " 34");
            }
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
