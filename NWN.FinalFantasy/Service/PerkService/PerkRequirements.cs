using System;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service.PerkService
{
    public interface IPerkPurchaseRequirement
    {
        bool CheckRequirements(uint player);
    }

    public interface IPerkActivationRequirement
    {
        void AfterActivationAction(uint player);
    }

    public class PerkSkillRequirement : IPerkPurchaseRequirement, IPerkActivationRequirement
    {
        private readonly SkillType _type;
        private readonly int _requiredRank;

        public PerkSkillRequirement(SkillType type, int requiredRank)
        {
            _type = type;
            _requiredRank = requiredRank;
        }

        public bool CheckRequirements(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var skill = dbPlayer.Skills[_type];
            var rank = skill.Rank;

            return rank >= _requiredRank;
        }

        public void AfterActivationAction(uint player)
        {
        }
    }

    public class PerkMPRequirement : IPerkActivationRequirement
    {
        private readonly int _requiredMP;

        public PerkMPRequirement(int requiredMP)
        {
            _requiredMP = requiredMP;
        }

        public bool CheckRequirements(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            return dbPlayer.MP > _requiredMP;
        }

        public void AfterActivationAction(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            Stat.ReduceMP(dbPlayer, _requiredMP);

            DB.Set(playerId, dbPlayer);
        }
    }

    public class PerkStaminaRequirement : IPerkActivationRequirement
    {
        private readonly int _requiredSTM;

        public PerkStaminaRequirement(int requiredSTM)
        {
            _requiredSTM = requiredSTM;
        }

        public bool CheckRequirements(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            return dbPlayer.Stamina > _requiredSTM;
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
