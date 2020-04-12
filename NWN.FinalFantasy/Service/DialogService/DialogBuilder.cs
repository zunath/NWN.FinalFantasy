using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.DialogService
{
    public class DialogBuilder
    {
        private string _defaultPageName;
        private DialogPage _activePage;
        private readonly List<DialogPage> _pages = new List<DialogPage>();
        private readonly List<Action> _initializationActions = new List<Action>();
        private readonly List<Action> _endActions = new List<Action>();
        private object _dataModel;

        public static DialogBuilder Create()
        {
            var builder = new DialogBuilder
            {
                _defaultPageName = "Page_1"
            };

            return builder;
        }

        public DialogBuilder AddInitializationAction(Action initializationAction)
        {
            _initializationActions.Add(initializationAction);

            return this;
        }

        public DialogBuilder WithDataModel(object dataModel)
        {
            _dataModel = dataModel;

            return this;
        }

        public DialogBuilder AddEndAction(Action endAction)
        {
            _endActions.Add(endAction);

            return this;
        }

        public DialogBuilder AddPage(string header)
        {
            var newPage = new DialogPage(header);
            _pages.Add(newPage);
            _activePage = newPage;

            return this;
        }

        public DialogBuilder AddResponse(string text, Action action)
        {
            Console.WriteLine($"firing addresponse = text = {text}");

            _activePage.AddResponse(text, action);
            return this;
        }

        public PlayerDialog Build()
        {
            var dialog = new PlayerDialog(_defaultPageName)
            {
                InitializationActions = _initializationActions,
                EndActions = _endActions,
                DataModel = _dataModel
            };

            var pageCount = 0;
            foreach (var page in _pages)
            {
                pageCount++;
                var dialogPage = dialog.AddPage(page.Header, $"Page_{pageCount}");
                dialogPage.Responses = page.Responses;
            }

            return dialog;
        }

    }
}
