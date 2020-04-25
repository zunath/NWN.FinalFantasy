using System;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service.PerkService
{
    public interface IPerkPurchaseRequirement
    {
        string CheckRequirements(uint player);
        string RequirementText { get; }
    }

    public interface IPerkActivationRequirement
    {
        bool UsedToCalculateEffectiveLevel { get; }
        string CheckRequirements(uint player);
        void AfterActivationAction(uint player);
    }

    /// <summary>
    /// Adds a minimum skill level as a requirement to purchase or activate a perk.
    /// </summary>
    public class PerkSkillRequirement : IPerkPurchaseRequirement, IPerkActivationRequirement
    {
        private readonly SkillType _type;
        private readonly int _requiredRank;

        public PerkSkillRequirement(SkillType type, int requiredRank)
        {
            _type = type;
            _requiredRank = requiredRank;
        }

        public bool UsedToCalculateEffectiveLevel => true;

        public string CheckRequirements(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var skill = dbPlayer.Skills[_type];
            var rank = skill.Rank;

            if (rank >= _requiredRank) return string.Empty;

            return $"Your skill rank is too low. (Your rank is {rank} versus required rank {_requiredRank})";
        }

        public string RequirementText
        {
            get
            {
                var skillDetails = Skill.GetSkillDetails(_type);
                return $"{skillDetails.Name} rank {_requiredRank}";
            }
        }

        public void AfterActivationAction(uint player)
        {
        }
    }

    public class PerkQuestRequirement : IPerkPurchaseRequirement
    {
        private readonly string _questId;

        public PerkQuestRequirement(string questId)
        {
            _questId = questId;
        }

        public string CheckRequirements(uint player)
        {
            var quest = Quest.GetQuestById(_questId);
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var error = $"You have not completed the quest '{quest.Name}'.";

            if (!dbPlayer.Quests.ContainsKey(_questId)) return error;

            var playerQuest = dbPlayer.Quests[_questId];
            if (playerQuest.TimesCompleted <= 0) return error;

            return string.Empty;
        }

        public string RequirementText
        {
            get
            {
                var quest = Quest.GetQuestById(_questId);
                return $"Quest: {quest.Name} Completed";
            }
        }
    }

    /// <summary>
    /// Adds an MP requirement to activate a perk.
    /// </summary>
    public class PerkMPRequirement : IPerkActivationRequirement
    {
        private readonly int _requiredMP;

        public PerkMPRequirement(int requiredMP)
        {
            _requiredMP = requiredMP;
        }

        public bool UsedToCalculateEffectiveLevel => false;

        public string CheckRequirements(uint player)
        {
            // NPCs and DMs are assumed to be able to activate.
            if (!GetIsPC(player) || GetIsDM(player)) return string.Empty;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            if (dbPlayer.MP > _requiredMP) return string.Empty;

            return $"Not enough MP. (Required: {_requiredMP})";
        }

        public void AfterActivationAction(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.ReduceMP(dbPlayer, _requiredMP);

            DB.Set(playerId, dbPlayer);
        }
    }

    /// <summary>
    /// Adds a stamina requirement to activate a perk.
    /// </summary>
    public class PerkStaminaRequirement : IPerkActivationRequirement
    {
        private readonly int _requiredSTM;

        public PerkStaminaRequirement(int requiredSTM)
        {
            _requiredSTM = requiredSTM;
        }

        public bool UsedToCalculateEffectiveLevel => false;

        public string CheckRequirements(uint player)
        {
            // NPCs and DMs are assumed to be able to activate.
            if (!GetIsPC(player) || GetIsDM(player)) return string.Empty;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            if (dbPlayer.Stamina > _requiredSTM) return string.Empty;
            return $"Not enough stamina. (Required: {_requiredSTM})";
        }

        public void AfterActivationAction(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.ReduceStamina(dbPlayer, _requiredSTM);

            DB.Set(playerId, dbPlayer);
        }
    }
}
