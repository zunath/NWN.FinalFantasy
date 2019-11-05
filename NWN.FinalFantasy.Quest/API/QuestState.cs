using System.Collections.Generic;
using NWN.FinalFantasy.Quest.API.Contracts;

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

        public bool IsComplete(NWGameObject player, int questID)
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
    }
}
