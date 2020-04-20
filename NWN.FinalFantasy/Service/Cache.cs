using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWNX.Enum;
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
        private static Dictionary<string, string> ItemNamesByResref { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Handles caching data into server memory for quicker lookup later.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void OnModuleLoad()
        {
            Console.WriteLine("Caching areas by resref.");
            CacheAreasByResref();

            Console.WriteLine("Caching item names by resref.");
            CacheItemNamesByResref();
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

        /// <summary>
        /// Stores the names of every item in the module. This speeds up the look-ups later on.
        /// </summary>
        private static void CacheItemNamesByResref()
        {
            var storageContainer = GetObjectByTag("temp_item_storage");
            var resref = Util.GetFirstResRef(ResRefType.Item);

            while (!string.IsNullOrWhiteSpace(resref))
            {
                var item = CreateItemOnObject(resref, storageContainer);
                ItemNamesByResref[resref] = GetName(item);
                DestroyObject(item);

                resref = Util.GetNextResRef();
            }
        }

        /// <summary>
        /// Retrieves the name of an item by its resref. If resref cannot be found, an empty string will be returned.
        /// </summary>
        /// <param name="resref">The resref to search for.</param>
        /// <returns>The name of an item, or an empty string if it cannot be found.</returns>
        public static string GetItemNameByResref(string resref)
        {
            if (!ItemNamesByResref.ContainsKey(resref))
                return string.Empty;

            return ItemNamesByResref[resref];
        }
    }
}
