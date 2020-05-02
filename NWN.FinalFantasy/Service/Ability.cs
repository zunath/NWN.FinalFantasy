using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Extension;
using NWN.FinalFantasy.Service.AbilityService;

namespace NWN.FinalFantasy.Service
{
    public static class Ability
    {
        private static readonly Dictionary<Feat, AbilityDetail> _abilities = new Dictionary<Feat, AbilityDetail>();

        // Recast Group Descriptions
        private static readonly Dictionary<RecastGroup, string> _recastDescriptions = new Dictionary<RecastGroup, string>();

        /// <summary>
        /// When the module loads, abilities will be cached.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void BuildCaches()
        {
            CacheRecastGroupNames();
            CacheAbilities();
        }

        private static void CacheAbilities()
        {
            // Organize perks to make later reads quicker.
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => typeof(IAbilityListDefinition).IsAssignableFrom(w) && !w.IsInterface && !w.IsAbstract);

            foreach (var type in types)
            {
                var instance = (IAbilityListDefinition) Activator.CreateInstance(type);
                var abilities = instance.BuildAbilities();

                foreach (var (feat, ability) in abilities)
                {
                    _abilities[feat] = ability;
                }
            }
        }

        /// <summary>
        /// Returns true if a feat is registered to an ability.
        /// Returns false otherwise.
        /// </summary>
        /// <param name="featType">The type of feat to check.</param>
        /// <returns>true if feat is registered to an ability. false otherwise.</returns>
        public static bool IsFeatRegistered(Feat featType)
        {
            return _abilities.ContainsKey(featType);
        }

        /// <summary>
        /// Retrieves an ability's details by the specified feat type.
        /// If feat does not have an ability, an exception will be thrown.
        /// </summary>
        /// <param name="featType">The type of feat</param>
        /// <returns>The ability detail</returns>
        public static AbilityDetail GetAbilityDetail(Feat featType)
        {
            if(!_abilities.ContainsKey(featType))
                throw new KeyNotFoundException($"Feat '{featType}' is not registered to an ability.");

            return _abilities[featType];
        }

        /// <summary>
        /// Reads all of the enum values on the RecastGroup enumeration and stores their short name into the cache.
        /// </summary>
        private static void CacheRecastGroupNames()
        {
            foreach (var recast in Enum.GetValues(typeof(RecastGroup)).Cast<RecastGroup>())
            {
                var attr = recast.GetAttribute<RecastGroup, RecastGroupAttribute>();
                _recastDescriptions[recast] = attr.ShortName;
            }
        }

        /// <summary>
        /// Retrieves the human-readable name of a recast group.
        /// </summary>
        /// <param name="recastGroup">The recast group to retrieve.</param>
        /// <returns>The name of a recast group.</returns>
        public static string GetRecastGroupName(RecastGroup recastGroup)
        {
            if (!_recastDescriptions.ContainsKey(recastGroup))
                throw new KeyNotFoundException($"Recast group {recastGroup} has not been registered. Did you forget the Description attribute?");

            return _recastDescriptions[recastGroup];
        }
    }
}
