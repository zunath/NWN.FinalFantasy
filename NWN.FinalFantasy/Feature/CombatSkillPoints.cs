using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public static class CombatSkillPoints
    {
        /// <summary>
        /// Tracks the combat points earned by players during combat.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, Dictionary<SkillType, int>>> _creatureCombatPointTracker = new Dictionary<string, Dictionary<string, Dictionary<SkillType, int>>>();

        /// <summary>
        /// Tracks the combat point lists associated with a player back to a creature.
        /// </summary>
        private static readonly Dictionary<string, HashSet<string>> _playerToCreatureTracker = new Dictionary<string, HashSet<string>>();


        [NWNEventHandler("mod_death")]
        public static void OnPlayerDeath()
        {
            
        }

        /// <summary>
        /// Adds a combat point to a given NPC creature for a given player and skill type.
        /// </summary>
        [NWNEventHandler("onhit_skill")]
        public static void OnHitCastSpell()
        {
            var player = OBJECT_SELF;
            if (!GetIsPC(player) || GetIsDM(player) || !GetIsObjectValid(player)) return;

            var item = GetSpellCastItem();
            var baseItemType = GetBaseItemType(item);
            var target = GetSpellTargetObject();
            if (GetIsPC(target) || GetIsDM(target)) return;

            var skill = Skill.GetSkillTypeByBaseItem(baseItemType);
            if (skill == SkillType.Unknown) return;

            AddCombatPoint(player, target, skill);
        }

        /// <summary>
        /// Handles clearing an NPC out of the combat point cache.
        /// </summary>
        [NWNEventHandler("crea_death")]
        public static void OnCreatureDeath()
        {
            var npc = OBJECT_SELF;
            var npcId = GetObjectUUID(npc);

            if (_creatureCombatPointTracker.ContainsKey(npcId))
            {
                // Remove references from the player-to-npc cache.
                foreach(var playerId in _creatureCombatPointTracker[npcId].Keys)
                {
                    RemovePlayerToNPCReferenceFromCache(playerId, npcId);
                }

                _creatureCombatPointTracker.Remove(npcId);
            }
        }

        /// <summary>
        /// When a player leaves an area or the server, we need to remove all combat points
        /// that may be referenced to their character.
        /// </summary>
        [NWNEventHandler("mod_exit")]
        [NWNEventHandler("area_exit")]
        public static void OnPlayerExit()
        {
            var player = GetExitingObject();
            if (!GetIsPC(player) || GetIsDM(player)) return;

            ClearPlayerCombatPoints(player);
        }

        /// <summary>
        /// Removes all combat points for a player as well as all other cache references.
        /// </summary>
        /// <param name="player">The player whose cache data we're removing</param>
        private static void ClearPlayerCombatPoints(uint player)
        {
            var playerId = GetObjectUUID(player);
            if (!_playerToCreatureTracker.ContainsKey(playerId)) return;

            // Loop over all npcIds the player has linked to them.
            var npcIds = _playerToCreatureTracker[playerId];
            foreach (var npcId in npcIds)
            {
                if (!_creatureCombatPointTracker.ContainsKey(npcId)) continue;

                if (!_creatureCombatPointTracker[npcId].ContainsKey(playerId)) continue;
                _creatureCombatPointTracker[npcId].Remove(playerId);
            }

            _playerToCreatureTracker.Remove(playerId);
        }

        /// <summary>
        /// Adds a combat point for a player to an NPC on a skill.
        /// </summary>
        /// <param name="player">The player receiving the point</param>
        /// <param name="creature">The creature to associate this point with.</param>
        /// <param name="skill">The skill to associate with the point.</param>
        private static void AddCombatPoint(uint player, uint creature, SkillType skill)
        {
            var playerId = GetObjectUUID(player);
            var npcId = GetObjectUUID(creature);
            var tracker = _creatureCombatPointTracker.ContainsKey(npcId) ?
                _creatureCombatPointTracker[npcId] :
                new Dictionary<string, Dictionary<SkillType, int>>();

            // Add an entry for this player if it doesn't exist.
            if (!tracker.ContainsKey(playerId))
            {
                tracker[playerId] = new Dictionary<SkillType, int>();
                AddPlayerToNPCReferenceToCache(player, creature);
            }

            // Add an entry for this skill if it doesn't exist.
            if (!tracker[playerId].ContainsKey(skill))
            {
                tracker[playerId][skill] = 0;
            }

            // Increment points for this player and skill by one.
            tracker[playerId][skill]++;

            _creatureCombatPointTracker[npcId] = tracker;
        }

        /// <summary>
        /// Adds a player-to-npc reference to the cache.
        /// </summary>
        /// <param name="player">The player whose reference we're attaching.</param>
        /// <param name="creature">The creature we're referencing</param>
        private static void AddPlayerToNPCReferenceToCache(uint player, uint creature)
        {
            var playerId = GetObjectUUID(player);
            var npcId = GetObjectUUID(creature);

            if(!_playerToCreatureTracker.ContainsKey(playerId))
            {
                _playerToCreatureTracker[playerId] = new HashSet<string>();
            }

            if(!_playerToCreatureTracker[playerId].Contains(npcId))
            {
                _playerToCreatureTracker[playerId].Add(npcId);
            }
        }

        /// <summary>
        /// Removes a player-to-npc reference from the cache.
        /// </summary>
        /// <param name="playerId">The player whose reference we're removing</param>
        /// <param name="npcId">The creature we're referencing</param>
        private static void RemovePlayerToNPCReferenceFromCache(string playerId, string npcId)
        {
            if (!_playerToCreatureTracker.ContainsKey(playerId)) return;

            if (_playerToCreatureTracker[playerId].Contains(npcId))
            {
                _playerToCreatureTracker[playerId].Remove(npcId);
            }
        }
    }
}
