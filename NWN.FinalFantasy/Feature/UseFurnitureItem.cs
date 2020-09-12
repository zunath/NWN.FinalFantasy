using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Feature.DialogDefinition;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Dialog = NWN.FinalFantasy.Service.Dialog;
using Object = NWN.FinalFantasy.Core.NWNX.Object;

namespace NWN.FinalFantasy.Feature
{
    public class UseFurnitureItem
    {
        /// <summary>
        /// Before an item is used, if it is a furniture item, cancel the event and start the furniture placement conversation
        /// </summary>
        [NWNEventHandler("item_use_bef")]
        public static void StartFurnitureConversation()
        {
            var player = OBJECT_SELF;
            var area = GetArea(player);
            var item = StringToObject(Events.GetEventData("ITEM_OBJECT_ID"));

            if (!Housing.CanPlaceFurniture(player, item))
            {
                return;
            }

            Events.SkipEvent();

            var furnitureTypeId = (int)Housing.GetFurnitureTypeFromItem(item);
            var targetLocation = Location(area,
                Vector(
                    (float)Convert.ToDouble(Events.GetEventData("TARGET_POSITION_X")),
                    (float)Convert.ToDouble(Events.GetEventData("TARGET_POSITION_Y")),
                    (float)Convert.ToDouble(Events.GetEventData("TARGET_POSITION_Z"))),
                0.0f);

            SetLocalInt(player, "TEMP_FURNITURE_TYPE_ID", furnitureTypeId);
            SetLocalObject(player, "TEMP_FURNITURE_OBJECT", item);
            SetLocalLocation(player, "TEMP_FURNITURE_LOCATION", targetLocation);
            Dialog.StartConversation(player, player, nameof(PlayerHouseFurnitureDialog));
        }
    }
}
