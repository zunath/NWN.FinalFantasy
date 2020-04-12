using System;
using System.Collections.Generic;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service.DialogService
{
    public abstract class DialogBase : IConversation
    {
        protected uint GetPC()
        {
            return GetPCSpeaker();
        }

        protected uint GetDialogTarget()
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dialog = Dialog.LoadPlayerDialog(playerId);
            return dialog.DialogTarget;
        }

        protected T GetDataModel<T>()
            where T: class
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dialog = Dialog.LoadPlayerDialog(playerId);
            return dialog.DataModel as T;
        }

        protected void ChangePage(string pageName, bool updateNavigationStack = true)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dialog = Dialog.LoadPlayerDialog(playerId);

            if (updateNavigationStack && dialog.EnableBackButton)
                dialog.NavigationStack.Push(new DialogNavigation(dialog.CurrentPageName, dialog.ActiveDialogName));
            dialog.CurrentPageName = pageName;
            dialog.PageOffset = 0;
        }

        protected void SwitchConversation(string conversationName)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dialog = Dialog.LoadPlayerDialog(playerId);
            Stack<DialogNavigation> navigationStack = null;

            if (dialog.EnableBackButton)
            {
                navigationStack = dialog.NavigationStack;
                navigationStack.Push(new DialogNavigation(dialog.CurrentPageName, dialog.ActiveDialogName));
            }
            Dialog.LoadConversation(GetPC(), dialog.DialogTarget, conversationName, dialog.DialogNumber);
            dialog = Dialog.LoadPlayerDialog(playerId);

            if (dialog.EnableBackButton && navigationStack != null)
                dialog.NavigationStack = navigationStack;

            dialog.ResetPage();
            ChangePage(dialog.CurrentPageName, false);

            foreach (var initializationAction in dialog.InitializationActions)
            {
                initializationAction();
            }

            SetLocalInt(player, "DIALOG_SYSTEM_INITIALIZE_RAN", 1);
        }

        protected void ToggleBackButton(bool isOn)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dialog = Dialog.LoadPlayerDialog(playerId);
            dialog.EnableBackButton = isOn;
            dialog.NavigationStack.Clear();
        }

        protected Stack<DialogNavigation> NavigationStack
        {
            get
            {
                var player = GetPC();
                var playerId = GetObjectUUID(player);
                var dialog = Dialog.LoadPlayerDialog(playerId);
                return dialog.NavigationStack;
            }
            set
            {
                var player = GetPC();
                var playerId = GetObjectUUID(player);
                var dialog = Dialog.LoadPlayerDialog(playerId);
                dialog.NavigationStack = value;
            }
        }

        protected void ClearNavigationStack()
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dialog = Dialog.LoadPlayerDialog(playerId);
            dialog.NavigationStack.Clear();
        }

        protected void EndConversation()
        {
            Dialog.EndConversation(GetPC());
        }

        public abstract PlayerDialog SetUp(uint player);
    }
}
