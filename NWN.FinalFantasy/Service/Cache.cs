using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    /// <summary>
    /// This class is responsible for loading and retrieving NWN data which lives for the lifespan of the server.
    /// Nothing in here will be permanently stored, it's simply here to make queries quicker.
    /// If you need persistent storage, refer to the DB class.
    /// </summary>
    public static class Cache
    {
        private static Dictionary<string, uint> AreasByResref { get; } = new Dictionary<string, uint>();

        /// <summary>
        /// Handles caching data into server memory for quicker lookup later.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void OnModuleLoad()
        {
            Console.WriteLine("Caching areas by resref.");
            CacheAreasByResref();
        }

        /// <summary>
        /// Caches all areas by their resref.
        /// </summary>
        private static void CacheAreasByResref()
        {
            for (var area = GetFirstArea(); GetIsObjectValid(area); area = GetNextArea())
            {
                var resref = GetResRef(area);
                AreasByResref[resref] = area;
            }
        }

        /// <summary>
        /// Retrieves an area by its resref. If the area does not exist, OBJECT_INVALID will be returned.
        /// </summary>
        /// <param name="resref">The resref to use for the search.</param>
        /// <returns>The area ID or OBJECT_INVALID if area does not exist.</returns>
        public static uint GetAreaByResref(string resref)
        {
            if (!AreasByResref.ContainsKey(resref))
                return Internal.OBJECT_INVALID;

            return AreasByResref[resref];
        }
    }
}
