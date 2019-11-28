using System;
using System.Linq;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Utility;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;

namespace NWN.FinalFantasy.Item.Storage
{
    public abstract class StorageBase
    {
        /// <summary>
        /// Retrieves the STORAGE_ID variable from the container.
        /// </summary>
        /// <returns>The value of the STORAGE_ID local variable</returns>
        protected static string GetStorageID()
        {
            var container = NWGameObject.OBJECT_SELF;

            var storageID = _.GetLocalString(container, "STORAGE_ID");

            if (string.IsNullOrWhiteSpace(storageID))
                throw new Exception($"Container {_.GetName(container)} does not have a STORAGE_ID variable assigned.");

            return storageID;
        }

        /// <summary>
        /// Retrieves the STORAGE_ITEM_LIMIT variable from the container.
        /// </summary>
        /// <returns>The value of the STORAGE_ITEM_LIMIT local variable</returns>
        protected static int GetItemLimit()
        {
            var limit = _.GetLocalInt(NWGameObject.OBJECT_SELF, "STORAGE_ITEM_LIMIT");
            if (limit <= 0)
                limit = 20;

            return limit;
        }

        /// <summary>
        /// Sends a message to a player informing them of the current number of items in the container and the maximum allowed.
        /// If incrementByOne is true, the current count will be increased by one. This is to account for the fact that
        /// the OnAddItem event fires before the item is actually added to the inventory (therefore it would have an off-by-one error)
        /// </summary>
        /// <param name="player">The player receiving the message</param>
        /// <param name="incrementByOne">Increments current item count by one if true, else does nothing.</param>
        protected static void SendItemLimitMessage(NWGameObject player, bool incrementByOne)
        {
            var container = NWGameObject.OBJECT_SELF;
            var limit = GetItemLimit();
            var count = _.GetInventoryItemCount(container);

            // The Add event fires before the item exists in the container. Need to increment by one in this scenario.
            if (incrementByOne)
                count++;

            _.SendMessageToPC(player, ColorToken.White("Item Limit: " + (count > limit ? limit : count) + " / ") + ColorToken.Red("" + limit));
        }

        /// <summary>
        /// Gets or sets the IS_LOADING local variable on the container.
        /// This is used to ensure the OnAddItem event does not process when the container is loading its items.
        /// </summary>
        protected static bool IsLoading
        {
            get => _.GetLocalInt(NWGameObject.OBJECT_SELF, "IS_LOADING") == 1;
            set => _.SetLocalInt(NWGameObject.OBJECT_SELF, "IS_LOADING", value ? 1 : 0);
        }

        /// <summary>
        /// Adds an item to the database under the specified key.
        /// </summary>
        /// <param name="key">The unique identifier under which this item list will be stored.</param>
        protected static void AddItem(string key)
        {
            // We don't want to serialize the item if we're loading its inventory.
            if (IsLoading) return;

            var container = NWGameObject.OBJECT_SELF;
            var item = NWNXEvents.OnInventoryAddItem_GetItem();
            var player = NWNXEvents.OnInventoryAddItem_GetPlayer();
            var limit = GetItemLimit();
            var count = _.GetInventoryItemCount(container);

            if (!_.GetIsPlayer(player))
            {
                CancelEvent(player, "Only players may store items here.");
                return;
            }

            if (_.GetHasInventory(item))
            {
                CancelEvent(player, "Containers cannot be stored.");
                return;
            }

            if (count >= limit)
            {
                CancelEvent(player, "No more items can be placed inside.");
                return;
            }

            if (_.GetBaseItemType(item) == BaseItemType.Gold)
            {
                CancelEvent(player, "Money cannot be placed inside.");
                return;
            }

            var items = InventoryItemRepo.Get(key);
            var itemID = _.GetGlobalID(item);
            var data = NWNXObject.Serialize(item);

            items.Entities.Add(new InventoryItem
            {
                ID = itemID,
                Data = data,
                Name = _.GetName(item),
                Quantity = _.GetItemStackSize(item),
                Resref = _.GetResRef(item),
                Tag = _.GetTag(item)
            });

            InventoryItemRepo.Set(key, items);
            SendItemLimitMessage(player, true);
        }

        /// <summary>
        /// Skips an event and sends a message to the player.
        /// Refer to NWNXEvents for information on which events may be skipped.
        /// Generally this is used in the OnAddItem event.
        /// </summary>
        /// <param name="player">The player who will receive the message.</param>
        /// <param name="message">The message sent</param>
        private static void CancelEvent(NWGameObject player, string message)
        {
            NWNXEvents.SkipEvent();
            _.SendMessageToPC(player, message);
        }

        /// <summary>
        /// Removes an item from the database by the specified key.
        /// </summary>
        /// <param name="key">The unique identifier for this item list.</param>
        protected static void RemoveItem(string key)
        {
            var player = _.GetLastDisturbed();
            var type = _.GetInventoryDisturbType();
            if (!_.GetIsPlayer(player)) return;
            if (type != InventoryDisturbType.Removed) return;

            var playerID = _.GetGlobalID(player);

            var storageID = GetStorageID();
            var items = InventoryItemRepo.Get(key);
            var item = _.GetInventoryDisturbItem();
            var itemID = _.GetGlobalID(item);

            var existing = items.Entities.FirstOrDefault(x => x.ID == itemID);
            if (existing == null)
                throw new Exception($"Could not locate item with ID '{itemID} from database for storage '{storageID}' and player ID {playerID}");

            items.Entities.Remove(existing);
            InventoryItemRepo.Set(key, items);
            SendItemLimitMessage(player, false);
        }

        /// <summary>
        /// Handles loading items into the container's inventory.
        /// </summary>
        /// <param name="key">The unique identifier under which this container's items are stored.</param>
        protected static void OpenStorage(string key)
        {
            var player = _.GetLastOpenedBy();
            if (!_.GetIsPlayer(player)) return;

            var container = NWGameObject.OBJECT_SELF;
            var items = InventoryItemRepo.Get(key);

            // Prevent the OnAddItem event from firing while we're loading the inventory.
            IsLoading = true;
            foreach (var entity in items.Entities)
            {
                var deserializedItem = NWNXObject.Deserialize(entity.Data);
                _.CopyItem(deserializedItem, container);
                _.DestroyObject(deserializedItem);
            }

            IsLoading = false;

            _.SetLocked(container, true);
            _.SendMessageToPC(player, "Move away from the container to close it.");
        }
    }
}
