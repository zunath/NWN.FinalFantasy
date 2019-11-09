using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.Event;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Event;
using NWN.FinalFantasy.Job.Message;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Menu
{
    internal class ChangeJobMenu: ConversationBase
    {
        public override void SetUp(NWGameObject player, PlayerDialog dialog)
        {
            DialogPage mainPage = new DialogPage("Please select a new job. Progress on your current job will be kept.");

            dialog.AddPage("MainPage", mainPage);
        }

        public override void Initialize()
        {
            var jobs = JobRegistry.GetAll();

            foreach (var job in jobs)
            {
                AddResponseToPage("MainPage", job.CallSign, true, job.Class);
            }
        }

        public override void DoAction(NWGameObject player, string pageName, int responseID)
        {
            switch (pageName)
            {
                case "MainPage":
                    MainPageResponses(responseID);
                    break;
            }
        }

        private void MainPageResponses(int responseID)
        {
            var player = GetPC();
            var playerID = GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);
            var response = GetResponseByID("MainPage", responseID);
            var classType = (ClassType)response.Data;

            var data = new JobChanged(player, entity.CurrentJob, classType);
            MessageHub.Instance.Publish(new CustomEvent(player, JobEventPrefix.OnJobChanged, data));
        }

        public override void Back(NWGameObject player, string beforeMovePage, string afterMovePage)
        {
        }

        public override void EndDialog()
        {
        }
    }
}
