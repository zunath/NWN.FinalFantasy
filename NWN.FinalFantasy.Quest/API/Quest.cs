using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.Event;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Quest.API.Contracts;
using NWN.FinalFantasy.Quest.API.Event;
using NWN.FinalFantasy.Quest.API.Prerequisite;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API
{
    internal class Quest
    {
        public string QuestID { get; protected set; }
        public string Name { get; protected set; }
        public string JournalTag { get; protected set; }
        public bool IsRepeatable { get; private set; }
        public bool AllowRewardSelection { get; private set; }

        private List<IQuestReward> Rewards { get; } = new List<IQuestReward>();
        private List<IQuestPrerequisite> Prerequisites { get; } = new List<IQuestPrerequisite>();

        private Dictionary<int, QuestState> States { get; } = new Dictionary<int, QuestState>();
        private readonly List<Action<NWGameObject, NWGameObject>> _onAcceptActions = new List<Action<NWGameObject, NWGameObject>>();
        private readonly List<Action<NWGameObject, NWGameObject, int>> _onAdvanceActions = new List<Action<NWGameObject, NWGameObject, int>>();
        private readonly List<Action<NWGameObject, NWGameObject>> _onCompleteActions = new List<Action<NWGameObject, NWGameObject>>();

        /// <summary>
        /// Adds a quest state to this quest.
        /// </summary>
        /// <returns>The newly created quest state.</returns>
        protected QuestState AddState()
        {
            int index = States.Count;
            States[index] = new QuestState();
            return States[index];
        }

        /// <summary>
        /// Retrieves a state by its index.
        /// </summary>
        /// <param name="state">The index of the state.</param>
        /// <returns>The quest state at a specified index</returns>
        protected QuestState GetState(int state)
        {
            return GetStates().ElementAt(state - 1);
        }

        /// <summary>
        /// Retrieves the list of quest states ordered by their sequence.
        /// </summary>
        /// <returns>A list of quest states ordered by their sequence</returns>
        protected IEnumerable<QuestState> GetStates()
        {
            return States.OrderBy(o => o.Key).Select(x => x.Value);
        }

        /// <summary>
        /// Adds an arbitrary action when this quest is accepted by a player.
        /// Actions execute in the order they were added.
        /// </summary>
        /// <param name="action">The action to add</param>
        protected void AddOnAcceptAction(Action<NWGameObject, NWGameObject> action)
        {
            _onAcceptActions.Add(action);
        }

        /// <summary>
        /// Adds an arbitrary action when this quest is advanced by a player.
        /// Actions execute in the order they were added.
        /// </summary>
        /// <param name="action">The action to add</param>
        protected void AddOnAdvanceAction(Action<NWGameObject, NWGameObject, int> action)
        {
            _onAdvanceActions.Add(action);
        }

        /// <summary>
        /// Adds an arbitrary action when this quest is completed by a player.
        /// Actions execute in the order they were added.
        /// </summary>
        /// <param name="action">The action to add</param>
        protected void AddOnCompleteAction(Action<NWGameObject, NWGameObject> action)
        {
            _onCompleteActions.Add(action);
        }

        /// <summary>
        /// Enables reward selection. Players will be able to pick which reward they want when this is enabled.
        /// </summary>
        protected void EnableRewardSelection()
        {
            AllowRewardSelection = true;
        }

        protected void EnableRepeatability()
        {
            IsRepeatable = true;
        }

        /// <summary>
        /// Returns true if player can accept this quest. Returns false otherwise.
        /// </summary>
        /// <param name="player">The player to check</param>
        /// <returns>true if player can accept, false otherwise</returns>
        private bool CanAccept(NWGameObject player)
        {
            // Retrieve the player's current quest status for this quest.
            // If they haven't accepted it yet, this will be null.
            var playerID = GetGlobalID(player);
            var status = QuestProgressRepo.Get(playerID, QuestID);

            // If the status is null, it's assumed that the player hasn't accepted it yet.
            if (status != null)
            {
                // If the quest isn't repeatable, prevent the player from accepting it after it's already been completed.
                if (status.TimesCompleted > 0)
                {
                    // If it's repeatable, then we don't care if they've already completed it.
                    if (!IsRepeatable)
                    {
                        SendMessageToPC(player, "You have already completed this quest.");
                        return false;
                    }
                }
                // If the player already accepted the quest, prevent them from accepting it again.
                else
                {
                    SendMessageToPC(player, "You have already accepted this quest.");
                    return false;
                }
            }

            // Check whether the player meets all necessary prerequisites.
            foreach (var prereq in Prerequisites)
            {
                if (!prereq.MeetsPrerequisite(player))
                {
                    SendMessageToPC(player, "You do not meet the prerequisites necessary to accept this quest.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if player can complete this quest. Returns false otherwise.
        /// </summary>
        /// <param name="player">The player to check</param>
        /// <returns>true if player can complete, false otherwise</returns>
        private bool CanComplete(NWGameObject player)
        {
            // Has the player even accepted this quest?
            var playerID = GetGlobalID(player);
            var pcStatus = QuestProgressRepo.Get(playerID, QuestID);

            if (pcStatus == null) return false;

            // Is the player on the final state of this quest?
            if (pcStatus.CurrentState != GetStates().Count()) return false;

            var state = GetState(pcStatus.CurrentState);
            // Are all objectives complete?
            foreach (var objective in state.GetObjectives())
            {
                if (!objective.IsComplete(player, QuestID))
                    return false;
            }

            // Met all requirements. We can complete this quest.
            return true;
        }

        /// <summary>
        /// Opens the reward selection menu wherein players can select the reward they want.
        /// If quest is not configured to allow reward selection, quest will be marked complete instead
        /// and all rewards will be given to the player.
        /// </summary>
        /// <param name="player">The player to request a reward from</param>
        /// <param name="questSource">The source of the quest reward giver</param>
        private void RequestRewardSelectionFromPC(NWGameObject player, NWGameObject questSource)
        {
            if (!GetIsPlayer(player)) return;

            if (AllowRewardSelection)
            {
                SetLocalString(player, "QST_REWARD_SELECTION_QUEST_ID", QuestID);
                Conversation.Start(player, player, "QuestRewardSelection");
            }
            else
            {
                Complete(player, questSource, null);
            }
        }

        /// <summary>
        /// Adds a new reward for completing this quest.
        /// </summary>
        /// <param name="reward">The new reward to add.</param>
        protected void AddReward(IQuestReward reward)
        {
            Rewards.Add(reward);
        }

        /// <summary>
        /// Adds a new prerequisite for accepting this quest.
        /// </summary>
        /// <param name="prerequisite">The new prerequisite to add.</param>
        protected void AddPrerequisite(IQuestPrerequisite prerequisite)
        {
            Prerequisites.Add(prerequisite);
        }

        /// <summary>
        /// Adds a new prerequisite quest which must be completed at least once before accepting this quest.
        /// </summary>
        /// <param name="requiredQuestID">The new prerequisite quest</param>
        protected void AddPrerequisiteQuest(string requiredQuestID)
        {
            AddPrerequisite(new RequiredQuestPrerequisite(requiredQuestID));
        }

        /// <summary>
        /// Returns the rewards given for completing this quest.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IQuestReward> GetRewards()
        {
            return Rewards;
        }

        /// <summary>
        /// Gives all rewards for this quest to the player.
        /// </summary>
        /// <param name="player">The player receiving the rewards.</param>
        internal void GiveRewards(NWGameObject player)
        {
            foreach (var reward in Rewards)
            {
                reward.GiveReward(player);
            }
        }

        /// <summary>
        /// Accepts a quest using the configured settings.
        /// </summary>
        /// <param name="player">The player accepting the quest.</param>
        /// <param name="questSource">The source of the quest giver</param>
        internal void Accept(NWGameObject player, NWGameObject questSource)
        {
            if (!GetIsPlayer(player)) return;

            if (!CanAccept(player))
            {
                return;
            }

            var playerID = GetGlobalID(player);

            // By this point, it's assumed the player will accept the quest.
            var status = QuestProgressRepo.Get(playerID, QuestID);

            // Retrieve the first quest state for this quest.
            status.CurrentState = 1;
            status.QuestID = QuestID;

            // Insert or update player's quest status.
            QuestProgressRepo.Set(playerID, status);

            var state = GetState(1);
            foreach (var objective in state.GetObjectives())
            {
                objective.Initialize(player, QuestID);
            }

            // Add the journal entry to the player.
            AddJournalQuestEntry(JournalTag, 1, player, false);

            // Notify them that they've accepted a quest.
            SendMessageToPC(player, "Quest '" + Name + "' accepted. Refer to your journal for more information on this quest.");

            // Run any quest-specific code.
            foreach (var action in _onAcceptActions)
            {
                action.Invoke(player, questSource);
            }

            // Notify to subscribers that a quest has just been accepted.
            var data = new QuestAccepted(player, QuestID);
            MessageHub.Instance.Publish(new CustomEvent(player, QuestEventPrefix.OnQuestAccepted, data));
        }

        /// <summary>
        /// Advances the player to the next quest state.
        /// </summary>
        /// <param name="player">The player advancing to the next quest state</param>
        /// <param name="questSource">The source of quest advancement</param>
        internal void Advance(NWGameObject player, NWGameObject questSource)
        {
            if (!GetIsPlayer(player)) return;

            // Retrieve the player's current quest state.
            var playerID = GetGlobalID(player);
            var questStatus = QuestProgressRepo.Get(playerID, QuestID);

            // Can't find a state? Notify the player they haven't accepted the quest.
            if (questStatus.CurrentState <= 0)
            {
                SendMessageToPC(player, "You have not accepted this quest yet.");
                return;
            }

            // If this quest has already been completed, exit early.
            // This is used in case a module builder incorrectly configures a quest.
            // We don't want to risk giving duplicate rewards.
            if (questStatus.TimesCompleted > 0 && !IsRepeatable) return;

            var currentState = GetState(questStatus.CurrentState);
            var lastState = GetStates().Last();

            // If this is the last state, the assumption is that it's time to complete the quest.
            if (currentState == lastState)
            {
                RequestRewardSelectionFromPC(player, questSource);
            }
            else
            {
                // Progress player's quest status to the next state.
                questStatus.CurrentState++;
                var nextState = GetState(questStatus.CurrentState);

                // Update the player's journal
                AddJournalQuestEntry(JournalTag, questStatus.CurrentState, player, false);

                // Notify the player they've progressed.
                SendMessageToPC(player, "Objective for quest '" + Name + "' complete! Check your journal for information on the next objective.");

                // Submit all of these changes to the cache/DB.
                QuestProgressRepo.Set(playerID, questStatus);

                // Create any extended data entries for the next state of the quest.
                foreach (var objective in nextState.GetObjectives())
                {
                    objective.Initialize(player, QuestID);
                }

                // Run any quest-specific code.
                foreach (var action in _onAdvanceActions)
                {
                    action.Invoke(player, questSource, questStatus.CurrentState);
                }

                // Notify to subscribers that the player has advanced to the next state of the quest.
                var data = new QuestAdvanced(player, QuestID, questStatus.CurrentState);
                MessageHub.Instance.Publish(new CustomEvent(player, QuestEventPrefix.OnQuestAdvanced, data));
            }

        }

        /// <summary>
        /// Completes a quest for a player. If a reward is selected, that reward will be given to the player.
        /// Otherwise, all rewards configured for this quest will be given to the player.
        /// </summary>
        /// <param name="player">The player completing the quest.</param>
        /// <param name="questSource">The source of the quest completion</param>
        /// <param name="selectedReward">The reward selected by the player</param>
        internal void Complete(NWGameObject player, NWGameObject questSource, IQuestReward selectedReward)
        {
            if (!GetIsPlayer(player)) return;
            if (!CanComplete(player)) return;

            var playerID = GetGlobalID(player);
            var pcState = QuestProgressRepo.Get(playerID, QuestID);

            // Mark player as being on the last state of the quest.
            pcState.CurrentState = GetStates().Count();
            pcState.TimesCompleted++;

            // No selected reward, simply give all available rewards to the player.
            if (selectedReward == null)
            {
                foreach (var reward in Rewards)
                {
                    reward.GiveReward(player);
                }
            }
            // There is a selected reward. Give that reward and any rewards which are not selectable to the player.
            else
            {
                // Non-selectable rewards (gold, GP, etc) are granted to the player.
                foreach (var reward in Rewards.Where(x => !x.IsSelectable))
                {
                    reward.GiveReward(player);
                }

                selectedReward.GiveReward(player);
            }

            QuestProgressRepo.Set(playerID, pcState);

            foreach (var action in _onCompleteActions)
            {
                action.Invoke(player, questSource);
            }

            SendMessageToPC(player,"Quest '" + Name + "' complete!");
            RemoveJournalQuestEntry(JournalTag, player, false);

            var data = new QuestCompleted(player, QuestID);
            MessageHub.Instance.Publish(new CustomEvent(player, QuestEventPrefix.OnQuestCompleted, data));
        }


    }
}
