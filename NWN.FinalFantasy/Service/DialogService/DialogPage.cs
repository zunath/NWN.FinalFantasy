using System;
using System.Collections.Generic;
using System.Text;

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

        public DialogPage(string header, params string[] responseOptions)
        {
            Responses = new List<DialogResponse>();
            Header = header;

            foreach (var response in responseOptions)
            {
                Responses.Add(new DialogResponse(response));
            }
        }

        public int NumberOfResponses => Responses.Count;
    }
}
