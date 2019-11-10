using System.Linq;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Quest.API.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API.Menu
{
    public class QuestRewardSelection : ConversationBase
    {
        private class Model
        {
            public string QuestID { get; set; }
        }

        public override void SetUp(NWGameObject player, PlayerDialog dialog)
        {
            DialogPage mainPage = new DialogPage(
                "Please select a reward."
            );

            dialog.AddPage("MainPage", mainPage);
        }

        public override void Initialize()
        {
            var pc = GetPC();
            string questID = GetLocalString(pc, "QST_REWARD_SELECTION_QUEST_ID");
            DeleteLocalString(pc, "QST_REWARD_SELECTION_QUEST_ID");
            var quest = QuestRegistry.GetQuest(questID); 
            var rewardItems = quest.GetRewards().Where(x => x.IsSelectable);

            Model model = new Model
            {
                QuestID = questID
            };
            SetDialogModel(model);

            foreach (var reward in rewardItems)
            {
                AddResponseToPage("MainPage", reward.MenuName, true, reward);
            }
        }

        public override void DoAction(NWGameObject player, string pageName, int responseID)
        {
            switch (pageName)
            {
                case "MainPage":
                    {
                        HandleRewardSelection(responseID);
                        break;
                    }
            }
        }

        public override void Back(NWGameObject player, string beforeMovePage, string afterMovePage)
        {
        }

        private void HandleRewardSelection(int responseID)
        {
            Model model = GetDialogModel<Model>();
            var reward = GetResponseByID("MainPage", responseID).Data as IQuestReward;
            var quest = QuestRegistry.GetQuest(model.QuestID);
            quest.Complete(GetPC(), GetPC(), reward);
            EndConversation();
        }

        public override void EndDialog()
        {
        }
    }
}
