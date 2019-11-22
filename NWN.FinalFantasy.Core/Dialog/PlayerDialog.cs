using System.Collections.Generic;

namespace NWN.FinalFantasy.Core.Dialog
{
    public class PlayerDialog
    {
        private Dictionary<string, DialogPage> Pages { get; set; }
        public string CurrentPageName { get; set; }
        public Stack<DialogNavigation> NavigationStack { get; set; }
        public int PageOffset { get; set; }
        public string ActiveDialogName { get; set; }
        public NWGameObject DialogTarget { get; set; }
        public object Data { get; set; }
        public bool IsEnding { get; set; }
        public string DefaultPageName { get; set; }
        public bool EnableBackButton { get; set; }
        public int DialogID { get; set; }

        public PlayerDialog(string defaultPageName)
        {
            Pages = new Dictionary<string, DialogPage>();
            CurrentPageName = string.Empty;
            NavigationStack = new Stack<DialogNavigation>();
            PageOffset = 0;
            ActiveDialogName = string.Empty;
            DefaultPageName = defaultPageName;
            EnableBackButton = true;
        }

        public void AddPage(string pageName, DialogPage page)
        {
            Pages.Add(pageName, page);
            if (Pages.Count == 1)
            {
                CurrentPageName = pageName;
            }
        }

        public DialogPage CurrentPage => Pages[CurrentPageName];

        public DialogPage GetPageByName(string pageName)
        {
            return Pages[pageName];
        }

        public void ResetPage()
        {
            CurrentPageName = DefaultPageName;
        }

    }
}