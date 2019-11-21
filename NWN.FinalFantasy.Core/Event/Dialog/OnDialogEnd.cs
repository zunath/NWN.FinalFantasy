using System;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class OnDialogEnd
    {
        internal static void Main()
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