using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWN.FinalFantasy.Core.Utility
{
    public static class TypeFinder
    {
        private static readonly ApplicationSettings _settings;

        static TypeFinder()
        {
            _settings = ApplicationSettings.Get();
        }

        /// <summary>
        /// Retrieves all of the concrete classes which implement the specified interface type.
        /// Abstract classes are excluded.
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <returns>A list of types which implement the specified interface.</returns>
        public static List<Type> GetTypesImplementingInterface<T>()
        {
            var type = typeof(T);

            if (!type.IsInterface)
                throw new Exception("Specified type 'T' must be an interface.");

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .ToList();

            return types;
        }

    }
}
