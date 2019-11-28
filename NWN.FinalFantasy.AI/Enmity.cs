using System;
using System.Collections.Generic;
using System.Linq;
using static NWN._;

namespace NWN.FinalFantasy.AI
{
    public static class Enmity
    {
        private static readonly Dictionary<Guid, EnmityTable> _enmityTables = new Dictionary<Guid, EnmityTable>();

        /// <summary>
        /// Retrieves or creates an enmity table for a specific creature.
        /// </summary>
        /// <param name="creature">The creature whose table we're retrieving or creating.</param>
        /// <returns>The enmity table used by the specified creature.</returns>
        private static EnmityTable GetOrCreateEnmityTable(NWGameObject creature)
        {
            if(!GetIsNPC(creature))
                throw new Exception(nameof(GetOrCreateEnmityTable) + " may only be used on NPCs.");

            var npcID = GetGlobalID(creature);
            if (!_enmityTables.ContainsKey(npcID))
            {
                _enmityTables[npcID] = new EnmityTable(creature);
            }

            return _enmityTables[npcID];
        }

        /// <summary>
        /// Retrieves the enmity rating a creature has towards the specified target.
        /// </summary>
        /// <param name="creature">The creature whose enmity we're checking</param>
        /// <param name="target">The target we're using for this check.</param>
        /// <returns>An enmity value that a creature feels towards a target.</returns>
        public static int GetEnmity(NWGameObject creature, NWGameObject target)
        {
            if(!GetIsNPC(creature))
                throw new Exception(nameof(GetEnmity) + " may only be used on NPCs.");

            var table = GetOrCreateEnmityTable(creature);
            var targetID = GetGlobalID(target);

            return !table.ContainsKey(targetID) ? 0 : table[targetID].Amount;
        }

        /// <summary>
        /// Increases or decreases a creature's enmity towards a target by adjustBy amount.
        /// </summary>
        /// <param name="creature">The creature whose enmity we're adjusting</param>
        /// <param name="target">The target we're using for the adjustment.</param>
        /// <param name="adjustBy">The amount to increase or decrease by. May be positive or negative.</param>
        public static void AdjustEnmity(NWGameObject creature, NWGameObject target, int adjustBy)
        {
            if (!GetIsNPC(creature))
                throw new Exception(nameof(AdjustEnmity) + " may only be used on NPCs.");

            var table = GetOrCreateEnmityTable(creature);
            var targetID = GetGlobalID(target);

            if(!table.ContainsKey(targetID))
                table[targetID] = new EnmityTarget(target, 0);

            table[targetID].Amount += adjustBy;
        }

        /// <summary>
        /// Removes an NPC's enmity table from the cache.
        /// </summary>
        /// <param name="npcID">The ID of the NPC whose enmity table we want to remove.</param>
        internal static void ClearEnmityTable(Guid npcID)
        {
            if (!_enmityTables.ContainsKey(npcID))
                return;

            _enmityTables.Remove(npcID);
        }
    }
}
