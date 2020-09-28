using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NWN.FinalFantasy.Core;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    public static class AI
    {
        private static volatile bool _isShuttingDown;
        private static ConcurrentBag<string> PlayerNames { get; set; } = new ConcurrentBag<string>();


        /// <summary>
        /// At module load, create the AI logic processing thread.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static async void RunThread()
        {
            var aiThread = Task.Run(() =>
            {
                AILogicAsync();
            });

            await NWTask.WhenAll(aiThread);
        }

        /// <summary>
        /// Simulates some long-running AI logic
        /// </summary>
        private static void AILogicAsync()
        {
            while (!_isShuttingDown)
            {
                Console.WriteLine($"AI thread loop start. Thread ID = {Thread.CurrentThread.ManagedThreadId}");

                foreach (var player in PlayerNames)
                {
                    Console.WriteLine("Name = " + player);
                }

                Thread.Sleep(5000);
                Console.WriteLine($"AI thread loop end. Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        /// <summary>
        /// Marks the application as shutting down, which will shut down the AI thread on the next iteration.
        /// </summary>
        [NWNEventHandler("app_shutdown")]
        public static void OnAppShutdown()
        {
            _isShuttingDown = true;
        }

        /// <summary>
        /// Update player names every heartbeat on the main thread.
        /// </summary>
        [NWNEventHandler("mod_heartbeat")]
        public static void UpdateNames()
        {
            PlayerNames.Clear();
            for (var player = GetFirstPC(); GetIsObjectValid(player); player = GetNextPC())
            {
                PlayerNames.Add(GetName(player));
            }
        }
    }
}
