using System;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Enumeration;
using NWN.FinalFantasy.Job.Event;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts.RecalculateStats
{
    internal abstract class RecalculateStatsBase: StatProcessingBase
    {
        protected static void Recalculate(NWGameObject player)
        {
            var playerID = GetGlobalID(player);
            var playerEntity = PlayerRepo.Get(playerID);
            var @class = GetClassByPosition(ClassPosition.First, player);
            var level = GetLevelByPosition(ClassPosition.First, player);
            var jobDefinition = JobRegistry.Get(@class);

            // Retrieve the rating chart for the stat, then retrieve the value for that stat at this player's level.
            var hp = RatingRegistry.Get(jobDefinition.HPRating).Get(RatingStat.HP, level);
            var mp = RatingRegistry.Get(jobDefinition.MPRating).Get(RatingStat.MP, level);
            var ac = RatingRegistry.Get(jobDefinition.ACRating).Get(RatingStat.AC, level) - 10; // Offset by the base 10
            var bab = RatingRegistry.Get(jobDefinition.BABRating).Get(RatingStat.BAB, level);

            var str = RatingRegistry.Get(jobDefinition.STRRating).Get(RatingStat.STR, level);
            var dex = RatingRegistry.Get(jobDefinition.DEXRating).Get(RatingStat.DEX, level);
            var con = RatingRegistry.Get(jobDefinition.CONRating).Get(RatingStat.CON, level);
            var wis = RatingRegistry.Get(jobDefinition.WISRating).Get(RatingStat.WIS, level);
            var @int = RatingRegistry.Get(jobDefinition.INTRating).Get(RatingStat.INT, level);
            var cha = RatingRegistry.Get(jobDefinition.CHARating).Get(RatingStat.CHA, level);

            // Now apply the changes to the player.
            ApplyHP(player, hp);
            playerEntity.MaxHP = hp;
            playerEntity.MaxMP = mp;
            playerEntity.HP = hp;
            playerEntity.MP = mp;

            NWNXCreature.SetBaseAC(player, ac);
            NWNXCreature.SetBaseAttackBonus(player, bab);

            NWNXCreature.SetRawAbilityScore(player, Ability.Strength, str);
            NWNXCreature.SetRawAbilityScore(player, Ability.Dexterity, dex);
            NWNXCreature.SetRawAbilityScore(player, Ability.Constitution, con);
            NWNXCreature.SetRawAbilityScore(player, Ability.Wisdom, wis);
            NWNXCreature.SetRawAbilityScore(player, Ability.Intelligence, @int);
            NWNXCreature.SetRawAbilityScore(player, Ability.Charisma, cha);

            PlayerRepo.Set(playerEntity);
            DelayCommand(1.0f, () => NWNXPlayer.UpdateCharacterSheet(player));

            Publish.CustomEvent(player, JobEventPrefix.OnStatsRecalculated, new StatsRecalculated(player));
        }

    }
}
