using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Object = NWN.FinalFantasy.Core.NWNX.Object;
using ObjectType = NWN.FinalFantasy.Core.NWScript.Enum.ObjectType;

namespace NWN.FinalFantasy.Service
{
    public static class PlayerMarket
    {
        // Serves two purposes:
        //    1.) Tracks the names of stores.
        //    2.) Identifies that the store is active and should be displayed on the shop list.
        private static Dictionary<string, string> ActiveStoreNames { get; } = new Dictionary<string, string>();

        // Tracks the merchant object which contains the items being sold by a store.
        private static Dictionary<string, uint> StoreMerchants { get; } = new Dictionary<string, uint>();

        /// <summary>
        /// When the module loads, look for all player stores and see if they're open and active.
        /// Those that are will be added to the cache for later look-up.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void LoadPlayerStores()
        {
            var keys = DB.SearchKeys("PlayerStore");

            foreach (var key in keys)
            {
                var dbPlayerStore = DB.Get<PlayerStore>(key);
                UpdateCacheEntry(key, dbPlayerStore);
            }
        }

        /// <summary>
        /// Determines if a store is currently open and if it should be displayed in the menu.
        /// </summary>
        /// <param name="store">The store to check.</param>
        /// <returns>true if store is available, false otherwise</returns>
        public static bool IsStoreOpen(PlayerStore store)
        {
            if (store.IsOpen &&
                DateTime.UtcNow < store.DateLeaseExpires)
                //store.ItemsForSale.Count > 0) // todo: add back
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves all of the active stores.
        /// </summary>
        /// <returns>A dictionary containing all of the active stores.</returns>
        public static Dictionary<string, string> GetAllActiveStores()
        {
            return ActiveStoreNames.ToDictionary(x => x.Key, y => y.Value);
        }

        public static void OpenPlayerStore(uint player, string storeOwnerPlayerId)
        {
            uint merchant;

            if (StoreMerchants.ContainsKey(storeOwnerPlayerId))
            {
                merchant = StoreMerchants[storeOwnerPlayerId];
            }
            else
            {
                var dbPlayerStore = DB.Get<PlayerStore>(storeOwnerPlayerId);
                merchant = CreateMerchantObject(dbPlayerStore);
                StoreMerchants[storeOwnerPlayerId] = merchant;
            }

            OpenStore(merchant, player);
        }

        private static uint CreateMerchantObject(PlayerStore dbPlayerStore)
        {
            const string StoreResref = "player_store";
            var merchant = CreateObject(ObjectType.Store, StoreResref, GetLocation(OBJECT_SELF));

            foreach (var item in dbPlayerStore.ItemsForSale)
            {
                var deserialized = Object.Deserialize(item.Value.Data);
                Object.AcquireItem(merchant, deserialized);
            }

            return merchant;
        }

        /// <summary>
        /// Updates the cache with the latest information from this entity.
        /// This should be called after changing a player store's details.
        /// </summary>
        /// <param name="playerId">The store owner's player Id</param>
        /// <param name="dbPlayerStore">The player store entity</param>
        public static void UpdateCacheEntry(string playerId, PlayerStore dbPlayerStore)
        {
            if (IsStoreOpen(dbPlayerStore))
            {
                ActiveStoreNames[playerId] = dbPlayerStore.StoreName;
            }
            else
            {
                if (ActiveStoreNames.ContainsKey(playerId))
                {
                    ActiveStoreNames.Remove(playerId);
                }
            }
        }

    }
}
