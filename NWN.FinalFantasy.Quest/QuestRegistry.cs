using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Quest
{
    internal static class QuestRegistry
    {
        private static readonly Dictionary<string, API.Quest> _quests = new Dictionary<string, API.Quest>();

        internal static void Register(API.Quest quest)
        {
            if (string.IsNullOrWhiteSpace(quest.QuestID))
                throw new Exception($"Quest ID is not set for quest type '{quest.GetType().Name}");
            if (string.IsNullOrWhiteSpace(quest.Name))
                throw new Exception($"Name is not set for quest type '{quest.GetType().Name}");
            if (string.IsNullOrWhiteSpace(quest.JournalTag))
                throw new Exception($"Journal Tag is not set for quest type '{quest.GetType().Name}");

            if (_quests.ContainsKey(quest.QuestID))
            {
                throw new Exception($"Quest with ID '{quest.QuestID}' has already been registered. Quest IDs must be unique.");
            }

            _quests[quest.QuestID] = quest;
        }

        public static API.Quest GetQuest(string questID)
        {
            return _quests[questID];
        }
    }
}
