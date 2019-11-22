using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Core.Dialog
{
    public class OnDialogAppears
    {
        internal static int Run(int nodeType, int nodeID)
        {
            var player = GetPCSpeaker();
            var playerID = GetGlobalID(player);
            var dialog = Conversation.GetActivePlayerDialog(playerID);

            DialogPage page = dialog.CurrentPage;
            var convo = Conversation.FindConversation(dialog.ActiveDialogName);
            int currentSelectionNumber = nodeID + 1;

            bool displayNode = false;
            string newNodeText = string.Empty;
            var gender = GetGender(player);
            int dialogOffset = (DialogConstants.NumberOfResponsesPerPage + 1) * (dialog.DialogID - 1);

            if (currentSelectionNumber == DialogConstants.NumberOfResponsesPerPage + 1) // Next page
            {
                int displayCount = page.NumberOfResponses - (DialogConstants.NumberOfResponsesPerPage * dialog.PageOffset);

                if (displayCount > DialogConstants.NumberOfResponsesPerPage)
                {
                    displayNode = true;
                }

                newNodeText = "Next";
            }
            else if (currentSelectionNumber == DialogConstants.NumberOfResponsesPerPage + 2) // Previous Page
            {
                if (dialog.PageOffset > 0)
                {
                    displayNode = true;
                }

                newNodeText = "Previous";
            }
            else if (currentSelectionNumber == DialogConstants.NumberOfResponsesPerPage + 3) // Back
            {
                if (dialog.NavigationStack.Count > 0 && dialog.EnableBackButton)
                {
                    displayNode = true;
                }

                newNodeText = "Back";
            }
            else if (nodeType == 2)
            {
                int responseID = (dialog.PageOffset * DialogConstants.NumberOfResponsesPerPage) + nodeID;

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
            else if (nodeType == 1)
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
                SetCustomToken(90000 + dialogOffset, newNodeText);
                return 1;
            }

            SetCustomToken(90001 + nodeID + dialogOffset, newNodeText);

            return displayNode ? 1 : 0;
        }
    }
}