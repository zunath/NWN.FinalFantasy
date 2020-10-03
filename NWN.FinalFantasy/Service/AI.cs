using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Extension;
using NWN.FinalFantasy.Service.AIService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    public static class AI
    {
        #region Main Thread Data
        
        private static int AIThreadFailures { get; set; }
        private static int _aiDataIterator;

        #endregion

        #region Shared Data (both threads)

        private static volatile bool _isShuttingDown;
        private static ConcurrentDictionary<string, AIInstructionSet> InstructionSets { get; } = new ConcurrentDictionary<string, AIInstructionSet>();
        private static ConcurrentDictionary<uint, AICreatureState> Creatures { get; } = new ConcurrentDictionary<uint, AICreatureState>();
        private static ConcurrentQueue<Tuple<uint, string>> AddQueue { get; } = new ConcurrentQueue<Tuple<uint, string>>();
        private static ConcurrentQueue<uint> RemovalQueue { get; } = new ConcurrentQueue<uint>();
        private static ConcurrentQueue<AICreatureCommand> CreatureCommandQueue { get; } = new ConcurrentQueue<AICreatureCommand>();
        private static ConcurrentBag<IAIDataUpdatable> AIUpdatables { get; } = new ConcurrentBag<IAIDataUpdatable>();
        private static ConcurrentBag<IAICreatureUpdatable> CreatureUpdatables { get; } = new ConcurrentBag<IAICreatureUpdatable>();

        #endregion

        #region Main NWN Thread (NWN Context is available)
        /// <summary>
        /// When the module loads, cache all AI data and create the AI logic processing thread.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static async void OnModuleLoad()
        {
            CacheData();
            Scheduler.ScheduleRepeating(UpdateSharedData, TimeSpan.FromSeconds(0.05d));
            Scheduler.ScheduleRepeating(ProcessCreatureCommandQueue, TimeSpan.FromSeconds(0.25d));
            await StartAIThreadAsync();
        }

        /// <summary>
        /// Handles updating shared data through a cycle. No more than one data set will refresh per call by design,
        /// to limit the hit on the main NWN thread.
        /// </summary>
        private static void UpdateSharedData()
        {
            using (new Profiler($"{nameof(AI)}:{nameof(UpdateSharedData)}"))
            {
                _aiDataIterator++;
                if (_aiDataIterator > AIUpdatables.Count - 1)
                {
                    _aiDataIterator = 0;
                }

                var aiData = AIUpdatables.ElementAt(_aiDataIterator);
                aiData.CaptureDataMainThread();
                aiData.WasUpdated = true;
            }
        }

        /// <summary>
        /// Each cycle through the main loop processes up to 100 creature commands. 
        /// </summary>
        private static void ProcessCreatureCommandQueue()
        {
            using (new Profiler($"{nameof(AI)}:{nameof(ProcessCreatureCommandQueue)}"))
            {
                const int MaxCommandsPerCycle = 500;
                var processedAmount = 0;

                while (CreatureCommandQueue.TryDequeue(out var command))
                {
                    // Creature is no longer valid.
                    if (!GetIsObjectValid(command.Creature) ||
                        !Creatures.ContainsKey(command.Creature))
                    {
                        RemovalQueue.Enqueue(command.Creature);
                    }
                    // Otherwise, process the action.
                    else
                    {
                        AssignCommand(command.Creature, () =>
                        {
                            command.Action.Action(command.Creature, command.CalculatedTargets.ToArray());
                        });

                        processedAmount++;
                    }

                    if (processedAmount >= MaxCommandsPerCycle)
                        break;
                }
            }
        }

        /// <summary>
        /// Loads all AI definitions and stores them into the cache.
        /// </summary>
        private static void CacheData()
        {
            // Use reflection to get all of the AI implementations.
            var classes = Assembly.GetCallingAssembly().GetTypes()
                .Where(p => typeof(IAIListDefinition).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToArray();
            foreach (var type in classes)
            {
                var instance = Activator.CreateInstance(type) as IAIListDefinition;
                if (instance == null)
                {
                    throw new NullReferenceException("Unable to activate instance of type: " + type);
                }

                var ais = instance.BuildAIs();

                foreach (var (key, value) in ais)
                {
                    InstructionSets[key] = value;
                }
            }

            // Do the same for AI updatables
            classes = Assembly.GetCallingAssembly().GetTypes()
                .Where(p => typeof(IAIDataUpdatable).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToArray();
            foreach (var type in classes)
            {
                var instance = Activator.CreateInstance(type) as IAIDataUpdatable;
                if (instance == null)
                {
                    throw new NullReferenceException("Unable to activate instance of type: " + type);
                }

                AIUpdatables.Add(instance);
            }

            // Do the same for Creature updatables
            classes = Assembly.GetCallingAssembly().GetTypes()
                .Where(p => typeof(IAICreatureUpdatable).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToArray();
            foreach (var type in classes)
            {
                var instance = Activator.CreateInstance(type) as IAICreatureUpdatable;
                if (instance == null)
                {
                    throw new NullReferenceException("Unable to activate instance of type: " + type);
                }

                CreatureUpdatables.Add(instance);
            }
        }

        /// <summary>
        /// Starts the AI thread which processes AI for all creatures in the background.
        /// </summary>
        /// <returns>A task containing the process</returns>
        private static async Task StartAIThreadAsync()
        {
            var aiThread = Task.Run(AILogicAsync);

            await NWTask.WhenAll(aiThread);
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
        /// When any creature spawns, queue it to be registered it with the AI service.
        /// </summary>
        [NWNEventHandler("crea_spawn")]
        public static void OnCreatureSpawn()
        {
            var creature = OBJECT_SELF;
            var instructionSetId = GetLocalString(creature, "AI_INSTRUCTION_SET_ID");
            if (!InstructionSets.ContainsKey(instructionSetId))
            {
                instructionSetId = "Generic";
            }

            AddQueue.Enqueue(new Tuple<uint, string>(creature, instructionSetId));

            foreach (var updatable  in CreatureUpdatables)
            {
                updatable.CreatureAddedMainThread(creature);
                updatable.WasUpdated = true;
            }
        }

        /// <summary>
        /// When any creature dies, queue it to be unregistered it with the AI service.
        /// </summary>
        [NWNEventHandler("crea_death")]
        public static void OnCreatureDeath()
        {
            var creature = OBJECT_SELF;
            RemovalQueue.Enqueue(creature);

            foreach (var updatable in CreatureUpdatables)
            {
                updatable.CreatureRemovedMainThread(creature);
                updatable.WasUpdated = true;
            }
        }

        #endregion

        #region AI Thread (NWN Context is NOT available)

        /// <summary>
        /// Processes the AI logic for all creatures.
        /// </summary>
        private static async Task AILogicAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Note: At no point should the NWN context be accessed in this thread. Doing so will cause crashes!
            // If you need data from NWN, store it into thread-safe variables on the main NWN thread (above).
            try
            {
                Log.Write(LogGroup.AI, "AI Thread Starting", true);
                var deltaTime = 0.0d;

                while (!_isShuttingDown)
                {
                    ProcessAIData();
                    ProcessCreatureRemovals();
                    ProcessCreatureAdditions();
                    ProcessCreatures(deltaTime);

                    deltaTime = stopwatch.Elapsed.TotalSeconds;

                    //if (stopwatch.ElapsedMilliseconds > 0)
                    //    Console.WriteLine($"AI thread: {stopwatch.ElapsedMilliseconds}ms");

                    stopwatch.Restart();
                }

                Log.Write(LogGroup.AI, "AI Thread Stopped", true);
            }
            catch (Exception ex)
            {
                AIThreadFailures++;
                Log.Write(LogGroup.AI, $"AI Thread Failed (Total Failures = {AIThreadFailures}): {ex.ToMessageAndCompleteStacktrace()}", true);

                if (AIThreadFailures < 10)
                {
                    Log.Write(LogGroup.AI, $"AI Thread is restarting due to failure...", true);
                    await AILogicAsync();
                }
            }
        }

        /// <summary>
        /// Handles processing recently updated AI data.
        /// This can be used to organize data sets on the AI thread rather than the main thread.
        /// </summary>
        private static void ProcessAIData()
        {
            foreach (var aiData in AIUpdatables)
            {
                if (aiData.WasUpdated)
                {
                    aiData.ProcessDataAIThread();
                    aiData.WasUpdated = false;
                }
            }
        }

        /// <summary>
        /// Processes the creature removal queue.
        /// </summary>
        private static void ProcessCreatureRemovals()
        {
            while (RemovalQueue.TryDequeue(out var creature))
            {
                if (Creatures.ContainsKey(creature))
                {
                    while (!Creatures.TryRemove(creature, out _))
                    {
                        // Empty on purpose. Keep trying to remove it until finished.
                    }
                }
            }
        }

        /// <summary>
        /// Processes the creature addition queue.
        /// </summary>
        private static void ProcessCreatureAdditions()
        {
            while (AddQueue.TryDequeue(out var creature))
            {
                if (!Creatures.ContainsKey(creature.Item1))
                {
                    Creatures[creature.Item1] = new AICreatureState(creature.Item2);
                }
            }
        }

        /// <summary>
        /// Iterates over all creatures and processes their AI.
        /// </summary>
        private static void ProcessCreatures(double deltaTime)
        {
            const double UpdateFrequency = 1.0d;

            Parallel.ForEach(Creatures, pair =>
            {
                var (creature, state) = pair;
                state.ProcessTime += deltaTime;

                if (state.ProcessTime >= UpdateFrequency)
                {
                    ProcessCreatureAI(creature, state.InstructionSetId);
                    state.ProcessTime = 0d;
                }
            });
        }

        /// <summary>
        /// Processes a specific creature's AI based on the instruction set they're using.
        /// </summary>
        /// <param name="creature">The creature to process</param>
        /// <param name="instructionSetId">The instruction set Id to use</param>
        private static void ProcessCreatureAI(uint creature, string instructionSetId)
        {
            var instructionSet = InstructionSets[instructionSetId];

            // Instructions are processed from top to bottom. As soon as a valid target is determined,
            // the action is performed.
            foreach (var instruction in instructionSet)
            {
                var meetsConditions = true;

                Parallel.ForEach(instruction.Action.Item1, (condition, state) =>
                {
                    if (!condition.MeetsCondition(creature))
                    {
                        meetsConditions = false;
                        state.Break();
                    }
                });

                if (meetsConditions)
                {
                    var targets = instruction.Targets.GetTargets(creature);
                    if (targets.Count > 0)
                    {
                        CreatureCommandQueue.Enqueue(new AICreatureCommand(creature, targets, instruction.Action.Item2));
                        break;
                    }
                }
            }
        }

        #endregion

    }
}
