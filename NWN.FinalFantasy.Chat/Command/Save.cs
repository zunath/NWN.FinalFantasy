namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Manually saves your character. Your character also saves automatically every few minutes.", CommandPermissionType.Player)]
    public class Save: IChatCommand
    {
        /// <summary>
        /// Exports user's character bic file.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            _.ExportSingleCharacter(user);
            _.SendMessageToPC(user, "Character saved successfully.");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
