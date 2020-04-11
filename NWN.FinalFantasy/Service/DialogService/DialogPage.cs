using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.DialogService
{
    public class DialogPage
    {
        public string Header { get; set; }
        public List<DialogResponse> Responses { get; set; }

        public DialogPage()
        {
            Responses = new List<DialogResponse>();
            Header = string.Empty;
        }

        public DialogPage(string header)
        {
            Responses = new List<DialogResponse>();
            Header = header;
        }

        public DialogPage AddResponse(string text, Action action)
        {
            Responses.Add(new DialogResponse(text, action));
            return this;
        }

        public int NumberOfResponses => Responses.Count;
    }
}
