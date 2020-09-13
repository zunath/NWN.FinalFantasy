using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Feature.DialogDefinition;
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
                if (item.Value.Price <= 0) continue;

                var deserialized = Object.Deserialize(item.Value.Data);
                Object.AcquireItem(merchant, deserialized);

                Core.NWNX.Item.SetBaseGoldPieceValue(deserialized, item.Value.Price);
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

        /// <summary>
        /// When an item is added to the terminal, track it and reopen the market terminal dialog.
        /// </summary>
        [NWNEventHandler("mkt_term_dist")]
        public static void MarketTerminalDisturbed()
        {
            if (GetInventoryDisturbType() != DisturbType.Added) return;

            var player = GetLastDisturbed();
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var dbPlayerStore = DB.Get<PlayerStore>(playerId);
            var item = GetInventoryDisturbItem();
            var itemId = GetObjectUUID(item);
            var serialized = Object.Serialize(item);
            var listingLimit = 5 + dbPlayer.SeedProgress.Rank * 5;

            SendMessageToPC(player, $"Listing limit: {dbPlayerStore.ItemsForSale.Count} / {5 + dbPlayer.SeedProgress.Rank * 5}");

            if (dbPlayerStore.ItemsForSale.Count >= listingLimit || // Listing limit reached.
                GetBaseItemType(item) == BaseItem.Gold ||           // Gold can't be listed.
                string.IsNullOrWhiteSpace(GetResRef(item)) ||       // Items without resrefs can't be listed.
                GetHasInventory(item))                              // Bags and other containers can't be listed.
            {
                Item.ReturnItem(player, item);
                SendMessageToPC(player, "This item cannot be listed.");
                return;
            }

            dbPlayerStore.ItemsForSale.Add(itemId, new PlayerStoreItem
            {
                Data = serialized,
                Name = GetName(item),
                Price = 0,
                StackSize = GetItemStackSize(item)
            });

            DB.Set(playerId, dbPlayerStore);
            DestroyObject(item);

        }

        /// <summary>
        /// When the terminal is opened, send an instructional message to the player.
        /// </summary>
        [NWNEventHandler("mkt_term_open")]
        public static void MarketTerminalOpened()
        {
            var player = GetLastOpenedBy();
            FloatingTextStringOnCreature("Place the items you wish to sell into the container. When you're finished, click the terminal again.", player, false);
        }

        /// <summary>
        /// When the terminal is closed, reset all event scripts on it.
        /// </summary>
        [NWNEventHandler("mkt_term_closed")]
        public static void MarketTerminalClosed()
        {
            var terminal = OBJECT_SELF;
            SetEventScript(terminal, EventScript.Placeable_OnOpen, string.Empty);
            SetEventScript(terminal, EventScript.Placeable_OnClosed, string.Empty);
            SetEventScript(terminal, EventScript.Placeable_OnInventoryDisturbed, string.Empty);
            SetEventScript(terminal, EventScript.Placeable_OnUsed, "start_convo");
        }

    }
}
