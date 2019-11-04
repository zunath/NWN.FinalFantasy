using System;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public abstract class MapPinBase
    {
        protected struct MapPinDetails
        {
            public int PlayerIndex { get; set; }
            public string Text { get; set; }
            public float PositionX { get; set; }
            public float PositionY { get; set; }
            public NWGameObject Area { get; set; }
            public string Tag { get; set; }
            public NWGameObject Player { get; set; }
            public int Index { get; set; }
        }

        protected static string BuildKey(Guid playerID, string areaResref)
        {
            return $"MapPin:{playerID}:{areaResref}";
        }

        protected static int GetNumberOfMapPins(NWGameObject player)
        {
            return GetLocalInt(player, "NW_TOTAL_MAP_PINS");
        }


        protected static MapPinDetails GetMapPinDetails(NWGameObject oPC, int index)
        {
            index++;
            MapPinDetails mapPin = new MapPinDetails()
            {
                Text = GetLocalString(oPC, "NW_MAP_PIN_NTRY_" + index),
                PositionX = GetLocalFloat(oPC, "NW_MAP_PIN_XPOS_" + index),
                PositionY = GetLocalFloat(oPC, "NW_MAP_PIN_YPOS_" + index),
                Area = GetLocalObject(oPC, "NW_MAP_PIN_AREA_" + index),
                Tag = GetLocalString(oPC, "CUSTOM_NW_MAP_PIN_TAG_" + index),
                Player = oPC,
                Index = index
            };

            return mapPin;
        }
    }
}
