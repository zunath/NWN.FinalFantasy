using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Player = NWN.FinalFantasy.Entity.Player;

namespace NWN.FinalFantasy.Service.QuestService
{
    public interface IQuestObjective
    {
        void Initialize(uint player, string questId);
        void Advance(uint player, string questId);
        bool IsComplete(uint player, string questId);
    }

    public class CollectItemObjective : IQuestObjective
    {
        private readonly string _resref;
        private readonly int _quantity;

        public CollectItemObjective(string resref, int quantity)
        {
            _resref = resref;
            _quantity = quantity;
        }

        public void Initialize(uint player, string questId)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var quest = dbPlayer.Quests.ContainsKey(questId) ? dbPlayer.Quests[questId] : new PlayerQuest();

            quest.ItemProgresses[_resref] = _quantity;
            dbPlayer.Quests[questId] = quest;
            DB.Set(playerId, dbPlayer);
        }

        public void Advance(uint player, string questId)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var quest = dbPlayer.Quests.ContainsKey(questId) ? dbPlayer.Quests[questId] : null;

            if (quest == null) return;
            if (!quest.ItemProgresses.ContainsKey(_resref)) return;
            if (quest.ItemProgresses[_resref] <= 0) return;

            quest.ItemProgresses[_resref]--;
            DB.Set(playerId, dbPlayer);

            var questDetail = Quest.GetQuestById(questId);
            var itemName = Cache.GetItemNameByResref(_resref);

            var statusMessage = $"[{questDetail.Name}] {itemName} remaining: {quest.ItemProgresses[_resref]}";

            if (quest.ItemProgresses[_resref] <= 0)
            {
                statusMessage += $" {ColorToken.Green("{COMPLETE}")}";
            }

            SendMessageToPC(player, statusMessage);
        }

        public bool IsComplete(uint player, string questId)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var quest = dbPlayer.Quests.ContainsKey(questId) ? dbPlayer.Quests[questId] : null;

            if (quest == null) return false;

            foreach (var progress in quest.ItemProgresses.Values)
            {
                if (progress > 0)
                    return false;
            }

            return true;
        }
    }

    public class KillTargetObjective : IQuestObjective
    {
        private readonly NPCGroupType _group;
        private readonly int _amount;

        public KillTargetObjective(NPCGroupType group, int amount)
        {
            _group = group;
            _amount = amount;
        }

        public void Initialize(uint player, string questId)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var quest = dbPlayer.Quests.ContainsKey(questId) ? dbPlayer.Quests[questId] : new PlayerQuest();
            
            quest.KillProgresses[_group] = _amount;
            dbPlayer.Quests[questId] = quest;
            DB.Set(playerId, dbPlayer);
        }

        public void Advance(uint player, string questId)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var quest = dbPlayer.Quests.ContainsKey(questId) ? dbPlayer.Quests[questId] : null;

            if (quest == null) return;
            if (!quest.KillProgresses.ContainsKey(_group)) return;
            if (quest.KillProgresses[_group] <= 0) return;

            quest.KillProgresses[_group]--;
            DB.Set(playerId, dbPlayer);

            var npcGroup = Quest.GetNPCGroup(_group);
            var questDetail = Quest.GetQuestById(questId);

            var statusMessage = $"[{questDetail.Name}] {npcGroup.Name} remaining: {quest.KillProgresses[_group]}";

            if (quest.KillProgresses[_group] <= 0)
            {
                statusMessage += $" {ColorToken.Green("{COMPLETE}")}";
            }

            SendMessageToPC(player, statusMessage);
        }

        public bool IsComplete(uint player, string questId)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var quest = dbPlayer.Quests.ContainsKey(questId) ? dbPlayer.Quests[questId] : null;

            if (quest == null) return false;

            foreach (var progress in quest.KillProgresses.Values)
            {
                if (progress > 0)
                    return false;
            }

            return true;
        }
    }
}
