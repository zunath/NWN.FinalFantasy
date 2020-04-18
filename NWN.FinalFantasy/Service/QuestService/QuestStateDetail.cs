using System;
using System.Collections.Generic;
using System.Text;

namespace NWN.FinalFantasy.Service.QuestService
{
    public class QuestStateDetail
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

        public bool IsComplete(uint player, string questID)
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
