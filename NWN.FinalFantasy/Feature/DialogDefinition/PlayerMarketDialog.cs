using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class PlayerMarketDialog: DialogBase
    {
        private class Model
        {
            public bool IsConfirmingExtendMaximum { get; set; }
            public bool IsConfirmingExtend7Days { get; set; }
            public bool IsConfirmingExtend1Day { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string ViewShopsPageId = "VIEW_SHOPS_PAGE";
        private const string EditMyShopPageId = "EDIT_MY_SHOPS_PAGE";
        private const string ChangeStoreNamePageId = "CHANGE_STORE_NAME_PAGE";
        private const string EditItemListPageId = "EDIT_ITEM_LIST_PAGE";
        private const string ExtendLeasePageId = "EXTEND_LEASE_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddPage(MainPageId, page =>
                {
                    page.Header = "Please select an option.";

                    page.AddResponse("View Shops", () =>
                    {
                        ChangePage(ViewShopsPageId);
                    });

                    page.AddResponse("Edit My Shop", () =>
                    {
                        ChangePage(EditMyShopPageId);
                    });
                })
                .AddPage(ViewShopsPageId, ViewShopsInit)
                .AddPage(EditMyShopPageId, EditMyShopInit)
                .AddPage(ChangeStoreNamePageId, ChangeStoreNameInit)
                .AddPage(EditItemListPageId, EditItemListInit)
                .AddPage(ExtendLeasePageId, ExtendLeaseInit)
                .AddBackAction((oldPage, newPage) =>
                {
                    ClearTemporaryVariables();
                })
                .AddEndAction(ClearTemporaryVariables);

            return builder.Build();
        }

        private void ClearTemporaryVariables()
        {
            var player = GetPC();
            var model = GetDataModel<Model>();

            model.IsConfirmingExtendMaximum = false;
            model.IsConfirmingExtend7Days = false;
            model.IsConfirmingExtend1Day = false;
            DeleteLocalString(player, "NEW_STORE_NAME");
            DeleteLocalBool(player, "IS_SETTING_STORE_NAME");
        }

        private void ViewShopsInit(DialogPage page)
        {
            page.Header = "Please select a shop.";

            var stores = PlayerMarket.GetAllActiveStores();

            foreach (var (key, name) in stores)
            {
                page.AddResponse(name, () =>
                {
                    PlayerMarket.OpenPlayerStore(GetPC(), key);
                    EndConversation();
                });
            }
        }

        private string GetLeaseStatus()
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerStore = DB.Get<PlayerStore>(playerId);
            var leaseStatus = DateTime.UtcNow > dbPlayerStore.DateLeaseExpires
                ? ColorToken.Red("EXPIRED")
                : dbPlayerStore.DateLeaseExpires.ToString("MM/dd/yyyy hh:mm:ss");

            return leaseStatus;
        }

        private void EditMyShopInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerStore = DB.Get<PlayerStore>(playerId) ?? new PlayerStore
            {
                StoreName = $"{GetName(player)}'s Store"
            };

            page.Header = ColorToken.Green("Store Name: ") + dbPlayerStore.StoreName + "\n" +
                ColorToken.Green("Lease Expires: ") + GetLeaseStatus() + "\n" +
                ColorToken.Green("Status: ") + (dbPlayerStore.IsOpen ? "OPEN" : "CLOSED") + "\n\n" +
                "Please select an option.";

            page.AddResponse("Change Store Name", () =>
            {
                ChangePage(ChangeStoreNamePageId);
            });

            page.AddResponse("Edit Item List", () =>
            {
                ChangePage(EditItemListPageId);
            });

            page.AddResponse(dbPlayerStore.IsOpen ? "Close Store" : "Open Store", () =>
            {
                dbPlayerStore.IsOpen = !dbPlayerStore.IsOpen;
                DB.Set(playerId, dbPlayerStore);
                PlayerMarket.UpdateCacheEntry(playerId, dbPlayerStore);
            });

            page.AddResponse("Extend Lease", () =>
            {
                ChangePage(ExtendLeasePageId);
            });

            DB.Set(playerId, dbPlayerStore);
        }

        [NWNEventHandler("on_nwnx_chat")]
        public static void ListenForStoreName()
        {
            var player = Chat.GetSender();
            if (!GetLocalBool(player, "IS_SETTING_STORE_NAME")) return;

            var message = Chat.GetMessage();

            if (message.Length > 30)
                message = message.Substring(0, 30);

            SetLocalString(player, "NEW_STORE_NAME", message);
            Chat.SkipMessage();

            FloatingTextStringOnCreature("Press 'Refresh' in the chat window to see the changes.", player, false);
        }

        private void ChangeStoreNameInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerStore = DB.Get<PlayerStore>(playerId) ?? new PlayerStore();
            var newStoreName = GetLocalString(player, "NEW_STORE_NAME");

            if (string.IsNullOrWhiteSpace(newStoreName))
                newStoreName = dbPlayerStore.StoreName;

            page.Header = ColorToken.Green("Store Name: ") + dbPlayerStore.StoreName + "\n" +
                ColorToken.Green("New Name: ") + newStoreName + "\n\n" +
                "Set your store's name by entering it in the chat bar. Press 'Refresh' to see changes.";

            page.AddResponse(ColorToken.Green("Refresh"), () => { });
            page.AddResponse("Set Name", () =>
            {
                dbPlayerStore.StoreName = newStoreName;
                DB.Set(playerId, dbPlayerStore);
                PlayerMarket.UpdateCacheEntry(playerId, dbPlayerStore);
            });

            SetLocalBool(player, "IS_SETTING_STORE_NAME", true);
        }

        private void EditItemListInit(DialogPage page)
        {

        }

        private void ExtendLeaseInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerStore = DB.Get<PlayerStore>(playerId);
            var model = GetDataModel<Model>();
            const int PricePerDay = 200;

            void ExtendLease(int days)
            {
                model.IsConfirmingExtendMaximum = false;
                model.IsConfirmingExtend7Days = false;
                model.IsConfirmingExtend1Day = false;

                var now = DateTime.UtcNow;

                // If expired, we start from the current time.
                if (now > dbPlayerStore.DateLeaseExpires)
                {
                    dbPlayerStore.DateLeaseExpires = now;
                }

                // Make sure the lease doesn't go over 30 days from now.
                if (dbPlayerStore.DateLeaseExpires.AddDays(days) > now.AddDays(30))
                {
                    var daysOver = dbPlayerStore.DateLeaseExpires.AddDays(days) - now.AddDays(30);
                    days -= daysOver.Days;
                }

                // Unable to add any more days to the lease. Must be at the maximum.
                if (days <= 0)
                {
                    FloatingTextStringOnCreature("Your lease cannot be extended past 30 days at a time.", player, false);
                    return;
                }

                // Player needs more money!
                if (GetGold(player) < days * PricePerDay)
                {
                    FloatingTextStringOnCreature("You do not have enough money to extend this lease.", player, false);
                    return;
                }

                TakeGoldFromCreature(days * PricePerDay, player, true);

                dbPlayerStore.DateLeaseExpires = dbPlayerStore.DateLeaseExpires.AddDays(days);
                DB.Set(playerId, dbPlayerStore);
                PlayerMarket.UpdateCacheEntry(playerId, dbPlayerStore);
            }

            page.Header = ColorToken.Green("Lease Expires: ") + GetLeaseStatus() + "\n\n" +
                $"Your store lease can be extended up to 30 days at a rate of {PricePerDay} gil per day.";

            if (model.IsConfirmingExtendMaximum)
            {
                page.AddResponse("CONFIRM EXTEND TO MAXIMUM", () =>
                {
                    ExtendLease(30);
                });
            }
            else
            {
                page.AddResponse("Extend to maximum", () =>
                {
                    model.IsConfirmingExtendMaximum = true;
                    model.IsConfirmingExtend7Days = false;
                    model.IsConfirmingExtend1Day = false;
                });
            }

            if (model.IsConfirmingExtend7Days)
            {
                page.AddResponse($"CONFIRM EXTEND BY 7 DAYS ({PricePerDay * 7} GIL)", () =>
                {
                    ExtendLease(7);
                });
            }
            else
            {
                page.AddResponse($"Extend by 7 days ({PricePerDay * 7} gil)", () =>
                {
                    model.IsConfirmingExtendMaximum = false;
                    model.IsConfirmingExtend7Days = true;
                    model.IsConfirmingExtend1Day = false;
                });
            }

            if (model.IsConfirmingExtend1Day)
            {
                page.AddResponse($"CONFIRM EXTEND BY 1 DAY ({PricePerDay * 1} GIL)", () =>
                {
                    ExtendLease(1);
                });
            }
            else
            {
                page.AddResponse($"Extend by 1 day ({PricePerDay * 1} gil)", () =>
                {
                    model.IsConfirmingExtendMaximum = false;
                    model.IsConfirmingExtend7Days = false;
                    model.IsConfirmingExtend1Day = true;
                });
            }
        }
    }
}
