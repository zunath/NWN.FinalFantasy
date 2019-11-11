using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.AbilityDefinition;

namespace NWN.FinalFantasy.Job.Registry
{
    internal class AbilityRegistry
    {
        private static readonly Dictionary<Feat, IAbilityDefinition> _abilityRegistry = new Dictionary<Feat, IAbilityDefinition>();
        private static readonly Dictionary<ClassType, List<IAbilityDefinition>> _abilitiesByJob = new Dictionary<ClassType, List<IAbilityDefinition>>();

        internal static void Register()
        {
            var type = typeof(IAbilityDefinition);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => type.IsAssignableFrom(x) && !x.IsInterface);

            foreach (var abilityType in types)
            {
                var ability = (IAbilityDefinition)Activator.CreateInstance(abilityType);
                _abilityRegistry[ability.Feat] = ability;

                foreach (var jobReq in ability.JobRequirements)
                {
                    if(!_abilitiesByJob.ContainsKey(jobReq.Job))
                        _abilitiesByJob[jobReq.Job] = new List<IAbilityDefinition>();

                    _abilitiesByJob[jobReq.Job].Add(ability);
                }

            }
        }

        /// <summary>
        /// Returns true if the specified feat is registered.
        /// </summary>
        /// <param name="feat">The feat to check the registration of</param>
        /// <returns>true if registered, false otherwise</returns>
        public static bool IsRegistered(Feat feat)
        {
            return _abilityRegistry.ContainsKey(feat);
        }

        /// <summary>
        /// Retrieves an ability by the feat from the registry.
        /// </summary>
        /// <param name="feat">The feat to use as the key</param>
        /// <returns>An ability associated with the specified feat</returns>
        public static IAbilityDefinition Get(Feat feat)
        {
            return _abilityRegistry[feat];
        }

        public static List<IAbilityDefinition> GetByJob(ClassType job)
        {
            return _abilitiesByJob[job];
        }
    }
}
