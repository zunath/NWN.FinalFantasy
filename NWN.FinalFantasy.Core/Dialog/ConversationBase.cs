using System;
using System.Collections.Generic;
using static NWN._;

namespace NWN.FinalFantasy.Core.Dialog
{
    public abstract class ConversationBase: IConversation
    {
        protected NWGameObject GetPC()
        {
            return (GetPCSpeaker());
        }

        private Guid GetPlayerID()
        {
            var player = GetPC();
            return GetGlobalID(player);
        }

        protected NWGameObject GetDialogTarget()
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            return dialog.DialogTarget;
        }

        protected T GetDialogModel<T>()
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            return (T)dialog.Data;
        }

        protected void SetDialogModel<T>(T value)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            dialog.Data = value;
        }

        protected void ChangePage(string pageName, bool updateNavigationStack = true)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());

            if (updateNavigationStack && dialog.EnableBackButton)
                dialog.NavigationStack.Push(new DialogNavigation(dialog.CurrentPageName, dialog.ActiveDialogName));
            dialog.CurrentPageName = pageName;
            dialog.PageOffset = 0;
        }

        protected void SetPageHeader(string pageName, string header)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            page.Header = header;
        }

        protected DialogPage GetPageByName(string pageName)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            return dialog.GetPageByName(pageName);
        }

        protected DialogPage GetCurrentPage()
        {
            return GetPageByName(GetCurrentPageName());
        }

        protected string GetCurrentPageName()
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            return dialog.CurrentPageName;
        }

        protected DialogResponse GetResponseByID(string pageName, int responseID)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            return page.Responses[responseID - 1];
        }

        protected void SetResponseText(string pageName, int responseID, string responseText)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            page.Responses[responseID - 1].Text = responseText;
        }

        protected void SetResponseVisible(string pageName, int responseID, bool isVisible)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            page.Responses[responseID - 1].IsActive = isVisible;
        }

        protected void AddResponseToPage(string pageName, string text, bool isVisible = true, object customData = null)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            page.Responses.Add(new DialogResponse(text, isVisible, customData));
        }

        protected void AddResponseToPage(string pageName, DialogResponse response)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            page.Responses.Add(response);
        }

        protected void ClearPageResponses(string pageName)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            DialogPage page = dialog.GetPageByName(pageName);
            page.Responses.Clear();
        }

        protected void SwitchConversation(string conversationName)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            Stack<DialogNavigation> navigationStack = null;

            if (dialog.EnableBackButton)
            {
                navigationStack = dialog.NavigationStack;
                navigationStack.Push(new DialogNavigation(dialog.CurrentPageName, dialog.ActiveDialogName));
            }
            Conversation.Load(GetPC(), dialog.DialogTarget, conversationName);
            dialog = Conversation.GetActivePlayerDialog(GetPlayerID());

            if (dialog.EnableBackButton && navigationStack != null)
                dialog.NavigationStack = navigationStack;

            dialog.ResetPage();
            ChangePage(dialog.CurrentPageName, false);

            var conversation = Conversation.FindConversation(dialog.ActiveDialogName);
            conversation.Initialize();
            SetLocalInt(GetPC(), "DIALOG_SYSTEM_INITIALIZE_RAN", 1);
        }

        protected void ToggleBackButton(bool isOn)
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            dialog.EnableBackButton = isOn;
            dialog.NavigationStack.Clear();
        }

        protected Stack<DialogNavigation> NavigationStack
        {
            get
            {
                PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
                return dialog.NavigationStack;
            }
            set
            {
                PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
                dialog.NavigationStack = value;
            }
        }

        protected void ClearNavigationStack()
        {
            PlayerDialog dialog = Conversation.GetActivePlayerDialog(GetPlayerID());
            dialog.NavigationStack.Clear();
        }

        protected void EndConversation()
        {
            Conversation.End(GetPC());
        }

        public abstract void SetUp(NWGameObject player, PlayerDialog dialog);
        public abstract void Initialize();
        public abstract void DoAction(NWGameObject player, string pageName, int responseID);
        public abstract void Back(NWGameObject player, string beforeMovePage, string afterMovePage);
        public abstract void EndDialog();
    }
}
