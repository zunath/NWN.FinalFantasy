using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Utility;
using NWN.FinalFantasy.Job.AbilityDefinition;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.Registry
{
    internal class AbilityRegistry
    {
        private static readonly Dictionary<Feat, IAbilityDefinition> _abilityRegistry = new Dictionary<Feat, IAbilityDefinition>();
        private static readonly Dictionary<ClassType, List<IAbilityDefinition>> _abilitiesByJob = new Dictionary<ClassType, List<IAbilityDefinition>>();

        internal static void Register()
        {
            var types = TypeFinder.GetTypesImplementingInterface<IAbilityDefinition>();
            foreach (var abilityType in types)
            {
                var ability = (IAbilityDefinition)Activator.CreateInstance(abilityType);
                _abilityRegistry[ability.Feat] = ability;

                // If the ability does not need to be manually equipped, add it to the list to be granted.
                if(ability.EquipStatus == EquipType.CrossJob ||
                   ability.EquipStatus == EquipType.SingleJob)
                {
                    foreach (var jobReq in ability.JobRequirements)
                    {
                        if (!_abilitiesByJob.ContainsKey(jobReq.Job))
                            _abilitiesByJob[jobReq.Job] = new List<IAbilityDefinition>();

                        _abilitiesByJob[jobReq.Job].Add(ability);
                    }
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
            return _abilitiesByJob[job].ToList();
        }
    }
}
