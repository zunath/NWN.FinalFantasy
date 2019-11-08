using System;
using System.Globalization;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Utility;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Contracts;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job
{
    internal class OnUseFeat
    {
        private struct UserStats
        {
            public NWGameObject User { get; set; }
            public NWGameObject Target { get; set; }
            public Location TargetLocation { get; set; }
            public int MP { get; set; }
            public int MPCost { get; set; }
            public IAbilityDefinition AbilityDefinition { get; set; }
            public DateTime Now { get; set; }
            public DateTime CooldownUnlock { get; set; }
            public bool IsCancelled { get; set; }
            public bool IsComplete { get; set; }
            public Vector CastingPosition { get; set; }
            public Feat Feat { get; set; }
        };

        public static void Main()
        {
            // Ignore any unregistered feats.
            var feat = NWNXEvents.OnFeatUsed_GetFeat();
            Console.WriteLine("Got feat = " + feat);

            if (!AbilityRegistry.IsRegistered(feat)) return;

            Console.WriteLine("getting target");
            var target = NWNXEvents.OnFeatUsed_GetTarget();

            Console.WriteLine("Getting user stats");
            // Retrieve stats and validate the use of this ability.
            var stats = GetUserStats(NWGameObject.OBJECT_SELF, target, feat);

            Console.WriteLine("validating feat use");
            if (!ValidateFeatUse(stats)) return;

            Console.WriteLine("starting casting process");
            // Begin the casting process
            StartCasting(stats);

            Console.WriteLine("done casting");
        }

        /// <summary>
        /// Validates whether all requirements to use ability are met.
        /// Returns true if successful, false otherwise.
        /// </summary>
        /// <param name="stats">The user's ability stats</param>
        /// <returns>true if successful, false otherwise</returns>
        private static bool ValidateFeatUse(UserStats stats)
        {
            var user = stats.User;
            var ability = stats.AbilityDefinition;
            var target = stats.Target;
            var canUse = ability.CanUse(user, target);

            if (stats.MP < stats.MPCost)
            {
                SendMessageToPC(user, ColorToken.Red("Insufficient MP."));
                return false;
            }

            if (!GetIsObjectValid(user))
            {
                return false;
            }

            if (!GetIsObjectValid(target))
            {
                SendMessageToPC(user, ColorToken.Red("Target lost."));
                return false;
            }

            if (!string.IsNullOrWhiteSpace(canUse))
            {
                SendMessageToPC(user, ColorToken.Red(canUse));
                return false;
            }

            if (stats.Now < stats.CooldownUnlock)
            {
                var timeToWait = Time.GetTimeToWaitLongIntervals(stats.Now, stats.CooldownUnlock, false);
                SendMessageToPC(user, ColorToken.Red($"Ability available in {timeToWait}."));
                return false;
            }

            if (!LineOfSightObject(user, target))
            {
                SendMessageToPC(user, ColorToken.Red($"You cannot see your target."));
                return false;
            }

            if (GetIsBusy(user) || GetCurrentHitPoints(user) <= 0 || !GetCommandable(user))
            {
                SendMessageToPC(user, ColorToken.Red("You are too busy."));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initializes the casting process.
        /// </summary>
        /// <param name="stats"></param>
        private static void StartCasting(UserStats stats)
        {
            var castingTime = stats.AbilityDefinition.CastingTime(stats.User);
            NWNXPlayer.StartGuiTimingBar(stats.User, castingTime, string.Empty);

            SetIsBusy(stats.User, true);

            DelayCommand(0.5f, () => CheckMovement(stats));
            DelayCommand(castingTime, () => FinishCasting(stats));
        }

        /// <summary>
        /// Checks caster's position. If they have moved too far from the initial casting position, the spell will be cancelled.
        /// </summary>
        /// <param name="stats">The user stats</param>
        private static void CheckMovement(UserStats stats)
        {
            if (stats.IsComplete) return;
            if (!GetIsObjectValid(stats.User)) return;

            var position = GetPosition(stats.User);

            // Player moved too far from starting position. Cancel the spell.
            if (Math.Abs(position.X - stats.CastingPosition.X) > 0.01f ||
                Math.Abs(position.Y - stats.CastingPosition.Y) > 0.01f ||
                Math.Abs(position.Z - stats.CastingPosition.Z) > 0.01f)
            {
                stats.IsCancelled = true;
                NWNXPlayer.StopGuiTimingBar(stats.User, string.Empty);
                return;
            }

            DelayCommand(0.5f, () => CheckMovement(stats));
        }

        /// <summary>
        /// Completes the casting process for a given spell.
        /// </summary>
        /// <param name="stats"></param>
        private static void FinishCasting(UserStats stats)
        {
            // Exit early if cancelled or validation fails.
            if (stats.IsCancelled) return;
            if (!ValidateFeatUse(stats)) return;

            // Ensure we mark this cast as complete.
            stats.IsComplete = true;
            SetIsBusy(stats.User, false);

            // Get the latest stats as other parts of the system may have changed our data since we started casting.
            stats = GetUserStats(stats.User, stats.Target, stats.Feat);
            stats.MP -= stats.MPCost;
            stats.AbilityDefinition.Impact(stats.User, stats.Target);

            // Apply changes to stats.
            ApplyUserStatChanges(stats);
        }

        /// <summary>
        /// Retrieves feat user stats. Data is retrieved differently based on whether user is a player or NPC.
        /// </summary>
        /// <returns>The stats of the user</returns>
        private static UserStats GetUserStats(NWGameObject user, NWGameObject target, Feat feat)
        {
            var ability = AbilityRegistry.Get(feat);
            var targetLocation = GetIsObjectValid(target) ? GetLocation(target) : NWNXEvents.OnFeatUsed_GetTargetLocation();

            var stats = new UserStats
            {
                User = user,
                AbilityDefinition = ability,
                Target = target,
                TargetLocation = targetLocation,
                Now = DateTime.UtcNow,
                CastingPosition = GetPosition(user),
                MPCost = ability.MP(user),
                Feat = feat
            };

            // Players - retrieve from DB
            if (GetIsPlayer(user))
            {
                var playerID = GetGlobalID(user);
                var player = PlayerRepo.Get(playerID);
                var cooldown = CooldownRepo.Get(playerID, feat);
                stats.MP = player.MP;
                stats.CooldownUnlock = cooldown.DateUnlocked;
            }
            // NPCs - retrieve from local variables
            else if (GetIsNPC(user))
            {
                stats.MP = GetLocalInt(user, "MP_CURRENT");

                var cooldown = GetLocalString(user, $"ABILITY_COOLDOWN_{feat}");
                if (string.IsNullOrWhiteSpace(cooldown))
                    stats.CooldownUnlock = DateTime.UtcNow;
                else
                    stats.CooldownUnlock = DateTime.ParseExact(cooldown, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
            }

            return stats;
        }

        /// <summary>
        /// Applies all changes in stats to the database or creature local variables.
        /// </summary>
        /// <param name="stats">The current user stats.</param>
        private static void ApplyUserStatChanges(UserStats stats)
        {
            var delay = stats.AbilityDefinition.CooldownTime(stats.User);
            var dateUnlocks = DateTime.UtcNow.AddSeconds(delay);
            if (GetIsPlayer(stats.User))
            {
                var playerID = GetGlobalID(stats.User);
                var player = PlayerRepo.Get(playerID);
                var cooldown = CooldownRepo.Get(playerID, stats.Feat);
                player.MP = stats.MP;

                PlayerRepo.Set(player);
                CooldownRepo.Set(playerID, cooldown);
            }
            else if (GetIsNPC(stats.User))
            {
                SetLocalInt(stats.User, "MP_CURRENT", stats.MP);
                SetLocalString(stats.User, $"ABILITY_COOLDOWN_{stats.Feat}", dateUnlocks.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture));
            }
        }
    }
}
