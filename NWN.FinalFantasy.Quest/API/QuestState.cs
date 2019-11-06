using System.Collections.Generic;
using NWN.FinalFantasy.Quest.API.Contracts;
using NWN.FinalFantasy.Quest.API.Objective;

namespace NWN.FinalFantasy.Quest.API
{
    internal class QuestState
    {
        private Dictionary<int, IQuestObjective> Objectives { get; } = new Dictionary<int, IQuestObjective>();

        public void AddObjective(IQuestObjective objective)
        {
            int index = Objectives.Count;
            Objectives[index] = objective;
        }

        public IEnumerable<IQuestObjective> GetObjectives()
        {
            return Objectives.Values;
        }

        public bool IsComplete(NWGameObject player, string questID)
        {
            foreach (var objective in Objectives)
            {
                if (!objective.Value.IsComplete(player, questID))
                {
                    return false;
                }
            }

            return true;
        }

        public QuestState AddObjectiveKillTarget(string group, int amount)
        {
            AddObjective(new KillTargetObjective(group, amount));
            return this;
        }

        public QuestState AddObjectiveCollectItem(string resref, int amount)
        {
            AddObjective(new CollectItemObjective(resref, amount));
            return this;
        }
    }
}
