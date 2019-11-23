namespace NWN.FinalFantasy.Core.Dialog
{
    public class OnDialogEnd
    {
        public static void Main()
        {
            var player = _.GetPCSpeaker();
            var playerID = _.GetGlobalID(player);
            var dialog = Conversation.GetActivePlayerDialog(playerID);
            var convo = Conversation.FindConversation(dialog.ActiveDialogName);
            convo.EndDialog();
            Conversation.End(player);
            _.DeleteLocalInt(player, "DIALOG_SYSTEM_INITIALIZE_RAN");
        }
    }
}