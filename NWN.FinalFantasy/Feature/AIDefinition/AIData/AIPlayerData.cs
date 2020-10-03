using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AIService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIData
{
    public class AIPlayerData: IAIDataUpdatable
    {
        public bool WasUpdated { get; set; }

        public class AIPlayer
        {
            public uint Player { get; set; }
            public int CurrentHP { get; set; }
            public int MaxHP { get; set; }
            public int CurrentMP { get; set; }
            public int MaxMP { get; set; }
            public int MaxSTM { get; set; }
            public int CurrentSTM { get; set; }
            public Vector3 Position { get; set; }
            public uint Area { get; set; }
            public float Facing { get; set; }
        }

        private static ConcurrentDictionary<uint, AIPlayer> PlayerData { get; } = new ConcurrentDictionary<uint,AIPlayer>();

        /// <summary>
        /// Loops through all players and retrieves information about them.
        /// </summary>
        public void CaptureDataMainThread()
        {
            using (new Profiler($"{nameof(AIPlayerData)}:{nameof(CaptureDataMainThread)}"))
            {
                PlayerData.Clear();

                for (var player = GetFirstPC(); GetIsObjectValid(player); player = GetNextPC())
                {
                    var playerId = GetObjectUUID(player);
                    var dbPlayer = DB.Get<Player>(playerId);

                    PlayerData[player] = new AIPlayer
                    {
                        Player = player,
                        CurrentHP = GetCurrentHitPoints(player),
                        MaxHP = GetMaxHitPoints(player),
                        MaxMP = Stat.GetMaxMP(player, dbPlayer),
                        CurrentMP = dbPlayer.MP,
                        MaxSTM = Stat.GetMaxStamina(player, dbPlayer),
                        CurrentSTM = dbPlayer.Stamina,
                        Position = GetPosition(player),
                        Area = GetArea(player),
                        Facing = GetFacing(player)
                    };
                }
            }
            
        }

        /// <summary>
        /// Recaches data when updates are received.
        /// </summary>
        public void ProcessDataAIThread()
        {
        }


        public static AIPlayer GetAIPlayer(uint player)
        {
            return PlayerData[player];
        }

        public static IEnumerable<AIPlayer> GetPlayersInArea(uint area)
        {
            return PlayerData.Where(x => x.Value.Area == area).Select(s => s.Value);
        }
    }
}
