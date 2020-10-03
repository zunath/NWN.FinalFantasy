using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NWN.FinalFantasy.Service.AIService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIData
{
    public class AICreatureData: IAIDataUpdatable, IAICreatureUpdatable
    {
        public bool WasUpdated { get; set; }

        public class AICreature
        {
            public uint Creature { get; set; }
            public int CurrentHP { get; set; }
            public int MaxHP { get; set; }
            public int CurrentMP { get; set; }
            public int MaxMP { get; set; }
            public int CurrentSTM { get; set; }
            public int MaxSTM { get; set; }
            public Vector3 Position { get; set; }
            public uint Area { get; set; }
            public float Facing { get; set; }
        }

        private static ConcurrentDictionary<uint, AICreature> Creatures { get; } = new ConcurrentDictionary<uint, AICreature>();

        public void CaptureDataMainThread()
        {
            
        }

        public void CreatureRemovedMainThread(uint creature)
        {
            while (!Creatures.TryRemove(creature, out _))
            {
                // Empty on purpose. Don't exit until it's removed.
            }
        }

        public void CreatureAddedMainThread(uint creature)
        {
            Creatures[creature] = new AICreature
            {
                Creature = creature,
                Area = GetArea(creature),
                CurrentHP = GetCurrentHitPoints(creature),
                MaxHP = GetMaxHitPoints(creature),
                CurrentMP = 0, // todo: get
                MaxMP = 0, // todo: get
                CurrentSTM = 0, // todo: get
                MaxSTM = 0, // todo: get
                Position = GetPosition(creature),
                Facing = GetFacing(creature)
            };
        }

        public void ProcessDataAIThread()
        {
        }

        /// <summary>
        /// Retrieves a specific creature from the AI data.
        /// Will throw an exception if creature is not found.
        /// </summary>
        /// <param name="creature">The creature to retrieve.</param>
        /// <returns>Data associated with a specified creature.</returns>
        public static AICreature GetCreature(uint creature)
        {
            return Creatures[creature];
        }

        /// <summary>
        /// Retrieves all of the creatures in a specific area.
        /// </summary>
        /// <param name="area">The area to search by.</param>
        /// <returns>An enumerable of AICreature data records.</returns>
        public static IEnumerable<AICreature> GetCreaturesInArea(uint area)
        {
            return Creatures.Where(x => x.Value.Area == area).Select(s => s.Value);
        }
    }
}
