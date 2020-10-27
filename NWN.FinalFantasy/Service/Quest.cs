﻿using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;
using NWN.FinalFantasy.Service.QuestService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Player = NWN.FinalFantasy.Entity.Player;

namespace NWN.FinalFantasy.Service
{
    public static class Quest
    {
        private static readonly Dictionary<string, QuestDetail> _quests = new Dictionary<string, QuestDetail>();
        private static readonly Dictionary<NPCGroupType, NPCGroupAttribute> _npcGroups = new Dictionary<NPCGroupType, NPCGroupAttribute>();
        private static readonly Dictionary<NPCGroupType, List<string>> _npcsWithKillQuests = new Dictionary<NPCGroupType, List<string>>();

        /// <summary>
        /// When the module loads, data is cached to speed up searches later.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void CacheData()
        {
            RegisterNPCGroups();
            RegisterQuests();
        }

        /// <summary>
        /// When the module loads, all quests will be retrieved with reflection and stored into a cache.
        /// </summary>
        public static void RegisterQuests()
        {
            // Organize quests to make later reads quicker.
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => typeof(IQuestListDefinition).IsAssignableFrom(w) && !w.IsInterface && !w.IsAbstract);

            foreach (var type in types)
            {
                var instance = (IQuestListDefinition) Activator.CreateInstance(type);
                var quests = instance.BuildQuests();

                foreach (var (questId, quest) in quests)
                {
                    _quests[questId] = quest;

                    // If any state has a Kill Target objective, add the NPC Group ID to the cache
                    foreach (var state in quest.States)
                    {
                        foreach (var objective in state.Value.GetObjectives())
                        {
                            if (objective is KillTargetObjective killObjective)
                            {
                                if(!_npcsWithKillQuests.ContainsKey(killObjective.Group))
                                    _npcsWithKillQuests[killObjective.Group] = new List<string>();

                                if(!_npcsWithKillQuests[killObjective.Group].Contains(questId))
                                    _npcsWithKillQuests[killObjective.Group].Add(questId);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// When the module loads, all of the NPCGroupTypes are iterated over and their data is stored into the cache.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void RegisterNPCGroups()
        {
            var npcGroups = Enum.GetValues(typeof(NPCGroupType)).Cast<NPCGroupType>();
            foreach (var npcGroupType in npcGroups)
            {
                var npcGroupDetail = npcGroupType.GetAttribute<NPCGroupType, NPCGroupAttribute>();
                _npcGroups[npcGroupType] = npcGroupDetail;
            }
        }

        /// <summary>
        /// When a player enters the module, load their quests.
        /// </summary>
        [NWNEventHandler("mod_enter")]
        public static void LoadPlayerQuests()
        {
            var player = GetEnteringObject();
            if (!GetIsPC(player) || GetIsDM(player)) return;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId) ?? new Player();

            // Reapply quest journal entries on log-in.
            // An NWN quirk requires this to be on a short delay because journal entries are wiped on login.
            DelayCommand(0.5f, () =>
            {
                foreach (var (questId, playerQuest) in dbPlayer.Quests)
                {
                    var quest = _quests[questId];
                    var state = quest.States[playerQuest.CurrentState];

                    Core.NWNX.Player.AddCustomJournalEntry(player, new JournalEntry
                    {
                        Name = quest.Name,
                        Text = state.JournalText,
                        Tag = questId,
                        State = playerQuest.CurrentState,
                        Priority = 1,
                        IsQuestCompleted = false,
                        IsQuestDisplayed = true,
                        Updated = 0,
                        CalendarDay = GetCalendarDay(),
                        TimeOfDay = GetTimeHour()
                    }, true);
                }
            });
        }

        /// <summary>
        /// Retrieves a quest by its Id. If the quest has not been registered, a KeyNotFoundException will be thrown.
        /// </summary>
        /// <param name="questId">The quest Id to search for.</param>
        /// <returns>The quest detail matching this Id.</returns>
        public static QuestDetail GetQuestById(string questId)
        {
            if(!_quests.ContainsKey(questId))
                throw new KeyNotFoundException($"Quest '{questId}' was not registered. Did you set the right Id?");

            return _quests[questId];
        }

        /// <summary>
        /// Retrieves an NPC group detail by the type.
        /// </summary>
        /// <param name="npcGroupType">The type of NPC group to retrieve.</param>
        /// <returns>An NPC group detail</returns>
        public static NPCGroupAttribute GetNPCGroup(NPCGroupType npcGroupType)
        {
            return _npcGroups[npcGroupType];
        }

        /// <summary>
        /// Retrieves the quests associated with an NPC group.
        /// If no quests are associated with this NPC group, an empty list will be returned.
        /// </summary>
        /// <param name="npcGroupType">The NPC group to search for</param>
        /// <returns>A list of quests associated with an NPC group.</returns>
        public static List<string> GetQuestsAssociatedWithNPCGroup(NPCGroupType npcGroupType)
        {
            if(!_npcsWithKillQuests.ContainsKey(npcGroupType))
                return new List<string>();

            return _npcsWithKillQuests[npcGroupType];
        }
            
        /// <summary>
        /// Makes a player accept a quest by the specified Id.
        /// If the quest Id is invalid, an exception will be thrown.
        /// </summary>
        /// <param name="player">The player who is accepting the quest</param>
        /// <param name="questId">The Id of the quest to accept.</param>
        public static void AcceptQuest(uint player, string questId)
        {
            _quests[questId].Accept(player, OBJECT_SELF);
        }

        /// <summary>
        /// Makes a player advance to the next state of the quest.
        /// If there are no additional states, the quest will be treated as completed.
        /// </summary>
        /// <param name="player">The player who is advancing to the next state of the quest.</param>
        /// <param name="questSource">The source of the quest. Typically an NPC or object.</param>
        /// <param name="questId">The Id of the quest to advance.</param>
        public static void AdvanceQuest(uint player, uint questSource, string questId)
        {
            _quests[questId].Advance(player, questSource);
        }
    }
}
