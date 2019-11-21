using System;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;
using static NWN.FinalFantasy.Core.Dialog.DialogConstants;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class OnDialogAppears
    {
        internal static int Main()
        {
            var player = GetPCSpeaker();
            var playerID = GetGlobalID(player);
            var dialog = Conversation.GetActivePlayerDialog(playerID);
            var nodeID = NWNXDialog.GetCurrentNodeIndex();
            var nodeType = NWNXDialog.GetCurrentNodeType();

            DialogPage page = dialog.CurrentPage;
            var convo = Conversation.FindConversation(dialog.ActiveDialogName);
            int currentSelectionNumber = nodeID + 1;

            bool displayNode = false;
            string newNodeText = string.Empty;
            var gender = GetGender(player);

            if (currentSelectionNumber == NumberOfResponsesPerPage + 1) // Next page
            {
                int displayCount = page.NumberOfResponses - (NumberOfResponsesPerPage * dialog.PageOffset);

                if (displayCount > NumberOfResponsesPerPage)
                {
                    displayNode = true;
                }

                newNodeText = "Next";
            }
            else if (currentSelectionNumber == NumberOfResponsesPerPage + 2) // Previous Page
            {
                if (dialog.PageOffset > 0)
                {
                    displayNode = true;
                }

                newNodeText = "Previous";
            }
            else if (currentSelectionNumber == NumberOfResponsesPerPage + 3) // Back
            {
                if (dialog.NavigationStack.Count > 0 && dialog.EnableBackButton)
                {
                    displayNode = true;
                }

                newNodeText = "Back";
            }
            else if (nodeType == DialogNodeType.Reply || nodeType == DialogNodeType.Entry)
            {
                int responseID = (dialog.PageOffset * NumberOfResponsesPerPage) + nodeID;

                if (responseID + 1 <= page.NumberOfResponses)
                {
                    DialogResponse response = page.Responses[responseID];

                    if (response != null)
                    {
                        newNodeText = response.Text;
                        displayNode = response.IsActive;
                    }
                }
            }
            else if (nodeType == DialogNodeType.Starting)
            {
                if (GetLocalInt(player, "DIALOG_SYSTEM_INITIALIZE_RAN") != 1)
                {
                    convo.Initialize();
                    SetLocalInt(player, "DIALOG_SYSTEM_INITIALIZE_RAN", 1);
                }

                if (dialog.IsEnding)
                {
                    convo.EndDialog();
                    Conversation.End(player);
                    DeleteLocalInt(player, "DIALOG_SYSTEM_INITIALIZE_RAN");

                    return 0;
                }

                page = dialog.CurrentPage;
                newNodeText = page.Header;
                NWNXDialog.SetCurrentNodeText(newNodeText, DialogLanguage.English, gender);

                return 1;
            }

            NWNXDialog.SetCurrentNodeText(newNodeText, DialogLanguage.English, gender);

            return displayNode ? 1 : 0;
        }
    }
}