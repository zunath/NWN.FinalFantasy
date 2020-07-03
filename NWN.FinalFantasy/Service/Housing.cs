using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Vector = NWN.FinalFantasy.Core.Vector;

namespace NWN.FinalFantasy.Service
{
    public static class Housing
    {
        private static readonly Dictionary<FurnitureType, FurnitureAttribute> _activeFurniture = new Dictionary<FurnitureType, FurnitureAttribute>();
        private static readonly Dictionary<PlayerHouseType, PlayerHouseAttribute> _activePlayerHouses = new Dictionary<PlayerHouseType, PlayerHouseAttribute>();
        private static readonly Dictionary<PlayerHouseType, Vector> _houseEntrancePositions = new Dictionary<PlayerHouseType, Vector>();

        /// <summary>
        /// When the module loads, cache all relevant data into memory.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void CacheData()
        {
            LoadFurniture();
            LoadPlayerHouses();
        }

        /// <summary>
        /// When the module loads, read all furniture types and store them into the cache.
        /// </summary>
        private static void LoadFurniture()
        {
            var furnitureTypes = Enum.GetValues(typeof(FurnitureType)).Cast<FurnitureType>();
            foreach (var furniture in furnitureTypes)
            {
                var furnitureDetail = furniture.GetAttribute<FurnitureType, FurnitureAttribute>();

                if (furnitureDetail.IsActive)
                {
                    _activeFurniture[furniture] = furnitureDetail;
                }
            }
        }

        /// <summary>
        /// When the module loads, read all player house types and store them into the cache.
        /// </summary>
        private static void LoadPlayerHouses()
        {
            var houseTypes = Enum.GetValues(typeof(PlayerHouseType)).Cast<PlayerHouseType>();
            foreach (var houseType in houseTypes)
            {
                var houseDetail = houseType.GetAttribute<PlayerHouseType, PlayerHouseAttribute>();

                if (houseDetail.IsActive)
                {
                    _activePlayerHouses[houseType] = houseDetail;
                }

                _houseEntrancePositions[houseType] = GetEntrancePosition(houseDetail.AreaInstanceResref);
            }
        }

        /// <summary>
        /// Iterates over all areas to find the matching instance assigned to the specified resref.
        /// Then, the entrance waypoint is located and its coordinates are stored into cache.
        /// </summary>
        /// <param name="areaResref">The resref of the area to look for</param>
        /// <returns>X, Y, and Z coordinates of the entrance location</returns>
        private static Vector GetEntrancePosition(string areaResref)
        {
            for (var area = GetFirstArea(); GetIsObjectValid(area); area = GetNextArea())
            {
                if (GetResRef(area) != areaResref) continue;

                for (var obj = GetFirstObjectInArea(area); GetIsObjectValid(obj); obj = GetNextObjectInArea(area))
                {
                    if (GetTag(obj) != "HOME_ENTRANCE") continue;

                    var position = GetPosition(obj);
                    return position;
                }
            }

            return new Vector();
        }

        /// <summary>
        /// Retrieves all of the active player house types.
        /// </summary>
        /// <returns>All active house types</returns>
        public static Dictionary<PlayerHouseType, PlayerHouseAttribute> GetActiveHouseTypes()
        {
            return _activePlayerHouses.ToDictionary(x => x.Key, y => y.Value);
        }

        /// <summary>
        /// Retrieves a specific type of house's details.
        /// </summary>
        /// <param name="type">The player house layout type.</param>
        /// <returns>Details for the specified house layout type.</returns>
        public static PlayerHouseAttribute GetHouseTypeDetail(PlayerHouseType type)
        {
            return _activePlayerHouses[type];
        }

        /// <summary>
        /// Retrieves the X, Y, and Z coordinates of the home layout's entrance.
        /// </summary>
        /// <param name="type">The type of house layout to look for</param>
        /// <returns>The X, Y, and Z coordinates of the home layout's entrance.</returns>
        public static Vector GetEntrancePosition(PlayerHouseType type)
        {
            return _houseEntrancePositions[type];
        }

        /// <summary>
        /// Stores the original location of a player, before being ported into a house instance.
        /// </summary>
        /// <param name="player">The player whose location will be stored.</param>
        public static void StoreOriginalLocation(uint player)
        {
            var position = GetPosition(player);
            var facing = GetFacing(player);
            var area = GetArea(player);

            SetLocalFloat(player, "HOUSING_STORED_LOCATION_X", position.X);
            SetLocalFloat(player, "HOUSING_STORED_LOCATION_Y", position.Y);
            SetLocalFloat(player, "HOUSING_STORED_LOCATION_Z", position.Z);
            SetLocalFloat(player, "HOUSING_STORED_LOCATION_FACING", facing);
            SetLocalObject(player, "HOUSING_STORED_LOCATION_AREA", area);
        }

        /// <summary>
        /// Jumps player to their original location, which is where they were before entering a house instance.
        /// This will also clear the temporary data related to the original location.
        /// </summary>
        /// <param name="player">The player who will jump.</param>
        public static void JumpToOriginalLocation(uint player)
        {
            var position = Vector(
                GetLocalFloat(player, "HOUSING_STORED_LOCATION_X"),
                GetLocalFloat(player, "HOUSING_STORED_LOCATION_Y"),
                GetLocalFloat(player, "HOUSING_STORED_LOCATION_Z"));
            var facing = GetLocalFloat(player, "HOUSING_STORED_LOCATION_FACING");
            var area = GetLocalObject(player, "HOUSING_STORED_LOCATION_AREA");
            var location = Location(area, position, facing);

            AssignCommand(player, () => ActionJumpToLocation(location));

            DeleteLocalFloat(player, "HOUSING_STORED_LOCATION_X");
            DeleteLocalFloat(player, "HOUSING_STORED_LOCATION_Y");
            DeleteLocalFloat(player, "HOUSING_STORED_LOCATION_Z");
            DeleteLocalFloat(player, "HOUSING_STORED_LOCATION_FACING");
            DeleteLocalObject(player, "HOUSING_STORED_LOCATION_AREA");
        }

        /// <summary>
        /// Creates an instance of an area and marks it as such.
        /// </summary>
        /// <param name="originalArea">The original area to copy.</param>
        /// <returns>The newly copied area.</returns>
        public static uint CreateInstance(uint originalArea)
        {
            var copy = CopyArea(originalArea);
            SetLocalBool(copy, "HOUSING_IS_INSTANCE", true);

            return copy;
        }

        /// <summary>
        /// Attempts to clean up an area instance.
        /// There must be no players in the area for this cleanup to happen and
        /// the area must be marked as an instance.
        /// It's recommended to call this on a short delay because players may
        /// still be considered in the area if they are transitioning between areas.
        /// </summary>
        /// <param name="area">The area to clean up.</param>
        public static void AttemptCleanUpInstance(uint area)
        {
            if (!GetLocalBool(area, "HOUSING_IS_INSTANCE")) return;

            DestroyArea(area);
        }
    }
}
