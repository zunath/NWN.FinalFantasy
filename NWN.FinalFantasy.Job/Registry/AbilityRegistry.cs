using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.AbilityDefinition;
using NWN.FinalFantasy.Job.Contracts;

namespace NWN.FinalFantasy.Job.Registry
{
    internal class AbilityRegistry
    {
        private static readonly Dictionary<Feat, IAbilityDefinition> _abilityRegistry = new Dictionary<Feat, IAbilityDefinition>();

        internal static void Register()
        {
            _abilityRegistry[Feat.TestAbility] = new TestAbility();
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
    }
}
