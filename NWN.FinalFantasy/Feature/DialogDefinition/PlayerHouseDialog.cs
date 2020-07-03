using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class PlayerHouseDialog : DialogBase
    {
        private class Model
        {
            public PlayerHouseType SelectedHouseType { get; set; }
            public bool IsConfirmingPurchase { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string PurchaseHouseLayoutDetailPageId = "PURCHASE_HOUSE_LAYOUT_DETAIL_PAGE";
        private const string SellHousePageId = "SELL_HOUSE_PAGE";
        

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddPage(MainPageId, MainPageInit)
                .AddPage(PurchaseHouseLayoutDetailPageId, PurchaseHouseLayoutDetailPageInit)
                .AddPage(SellHousePageId, SellHousePageInit);

            return builder.Build();
        }

        /// <summary>
        /// Loads the main page header and response options.
        /// </summary>
        /// <param name="page">The dialog page to adjust.</param>
        private void MainPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var hasHouse = DB.Exists<PlayerHouse>(playerId);

            // Player currently owns a house.
            // Provide options to adjust existing house.
            if (hasHouse)
            {
                var house = DB.Get<PlayerHouse>(playerId);
                var detail = Housing.GetHouseTypeDetail(house.HouseType);

                page.Header = ColorToken.Green("Layout Type: ") + detail.Name + "\n" +
                              ColorToken.Green("Furniture Limit: ") + house.Furnitures.Count + " / " + detail.FurnitureLimit + "\n\n" +
                              "What would you like to do?";

                page.AddResponse("Enter", () =>
                {
                    var instance = Housing.LoadPlayerHouse(playerId);
                    var entrancePosition = Housing.GetEntrancePosition(house.HouseType);
                    var location = Location(instance, entrancePosition, 0.0f);
                    Housing.StoreOriginalLocation(player);
                    
                    AssignCommand(player, () => ActionJumpToLocation(location));
                });

                page.AddResponse("Sell Home", () =>
                {
                    ChangePage(SellHousePageId);
                });
            }
            // Player doesn't own a house.
            // Provide options to buy one.
            else
            {
                PurchaseHouseLayoutListPageInit(page);
            }

        }

        /// <summary>
        /// Handles the "Purchase List Page" header and responses.
        /// </summary>
        /// <param name="page">The page we're building.</param>
        private void PurchaseHouseLayoutListPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var seedRank = dbPlayer.SeedProgress.Rank;
            var availableLayouts = Housing.GetActiveHouseTypes();
            var model = GetDataModel<Model>();

            page.Header = "You can purchase any of the following layouts. Please note that this list is restricted based on your current SeeD rank. Complete missions to raise your SeeD rank and unlock new layouts.\n\n" +
                ColorToken.Green("Your SeeD Rank: ") + seedRank;

            foreach (var layout in availableLayouts)
            {
                // Player's SeeD rank isn't high enough to purchase this layout.
                if (seedRank < layout.Value.RequiredSeedRank) continue;

                page.AddResponse(layout.Value.Name, () =>
                {
                    model.SelectedHouseType = layout.Key;
                    ChangePage(PurchaseHouseLayoutDetailPageId);
                });
            }

        }

        /// <summary>
        /// Handles the "Purchase Detail Page" header and responses.
        /// </summary>
        /// <param name="page">The page we're building.</param>
        private void PurchaseHouseLayoutDetailPageInit(DialogPage page)
        {
            var player = GetPC();
            var model = GetDataModel<Model>();
            var layoutDetail = Housing.GetHouseTypeDetail(model.SelectedHouseType);

            void PurchaseHome()
            {
                var playerId = GetObjectUUID(player);
                var playerHouse = new PlayerHouse
                {
                    HouseType = model.SelectedHouseType
                };

                DB.Set(playerId, playerHouse);

                FloatingTextStringOnCreature("You've purchased a new home!", player, false);
            }

            page.Header = ColorToken.Green("Layout: ") + layoutDetail.Name + "\n" +
                ColorToken.Green("Required SeeD Rank: ") + layoutDetail.RequiredSeedRank + "\n" +
                ColorToken.Green("Price: ") + layoutDetail.Price + " gil\n" +
                ColorToken.Green("Furniture Limit: ") + layoutDetail.FurnitureLimit + " items";

            page.AddResponse("Preview", () =>
            {
                var originalArea = Cache.GetAreaByResref(layoutDetail.AreaInstanceResref);
                if (originalArea == OBJECT_INVALID)
                {
                    Log.Write(LogGroup.Error, $"{GetName(player)} attempted to preview a house layout with resref {layoutDetail.AreaInstanceResref} but the area could not be found. Ensure an area has been created in the module.");
                    return;
                }

                var copy = Housing.CreateInstance(originalArea);
                var position = Housing.GetEntrancePosition(model.SelectedHouseType);
                var location = Location(copy, position, 0.0f);
                SetName(copy, $"[PREVIEW] {GetName(copy)}");

                Housing.StoreOriginalLocation(player);
                AssignCommand(player, () => ActionJumpToLocation(location));
            });

            // Purchase/confirm purchase options only display if player has enough gold.
            if (GetGold(player) >= layoutDetail.Price)
            {
                // Is confirming purchase
                if (model.IsConfirmingPurchase)
                {
                    page.AddResponse($"CONFIRM PURCHASE ({layoutDetail.Price} gil)", () =>
                    {
                        model.IsConfirmingPurchase = false;

                        // Need to check gold again, in case they dropped money to the ground while the conversation was open.
                        if (GetGold(player) < layoutDetail.Price)
                        {
                            FloatingTextStringOnCreature("You do not have enough gil to purchase this home.", player, false);
                            return;
                        }

                        PurchaseHome();

                        EndConversation();
                    });
                }
                else
                {
                    page.AddResponse($"Purchase ({layoutDetail.Price} gil)", () =>
                    {
                        model.IsConfirmingPurchase = true;
                    });
                }
            }

        }

        /// <summary>
        /// Handles the "Sell House Page" header and responses.
        /// </summary>
        /// <param name="page">The page we're building.</param>
        private void SellHousePageInit(DialogPage page)
        {

        }
    }
}
