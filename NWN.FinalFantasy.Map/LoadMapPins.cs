using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class LoadMapPins: MapPinBase
    {
        public static void Main()
        {
            NWGameObject oPC = (GetEnteringObject());

            if (!GetIsPlayer(oPC)) return;
            if (GetLocalInt(oPC, "MAP_PINS_LOADED") == 1) return;

            var playerID = GetGlobalID(oPC);
            var pins = MapPinRepo.Get(playerID);

            foreach (var pin in pins.Entities)
            {
                var area = GetObjectByTag(pin.AreaTag);
                SetMapPin(oPC, pin.Text, pin.PositionX, pin.PositionY, area);
            }

            SetLocalInt(oPC, "MAP_PINS_LOADED", 1);
        }



        private static void SetMapPin(NWGameObject oPC, string text, float positionX, float positionY, NWGameObject area)
        {
            int numberOfMapPins = GetNumberOfMapPins(oPC);
            int storeAtIndex = -1;

            for (int index = 0; index < numberOfMapPins; index++)
            {
                var mapPin = GetMapPinDetails(oPC, index);
                if (string.IsNullOrWhiteSpace(mapPin.Text))
                {
                    storeAtIndex = index;
                    break;
                }
            }

            if (storeAtIndex == -1)
            {
                numberOfMapPins++;
                storeAtIndex = numberOfMapPins - 1;
            }

            storeAtIndex++;

            SetLocalString(oPC, "NW_MAP_PIN_NTRY_" + storeAtIndex, text);
            SetLocalFloat(oPC,"NW_MAP_PIN_XPOS_" + storeAtIndex, positionX);
            SetLocalFloat(oPC, "NW_MAP_PIN_YPOS_" + storeAtIndex, positionY);
            SetLocalObject(oPC, "NW_MAP_PIN_AREA_" + storeAtIndex, area);
            SetLocalInt(oPC,"NW_TOTAL_MAP_PINS", numberOfMapPins);
        }
    }
}
