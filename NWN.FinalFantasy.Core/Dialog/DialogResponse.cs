namespace NWN.FinalFantasy.Core.Dialog
{
    public class DialogResponse
    {
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public object Data { get; set; }

        public DialogResponse(string text, bool isVisible = true, object data = null)
        {
            Text = text;
            IsActive = isVisible;
            Data = data;
        }

        public bool HasCustomData => Data != null;

    }
}
