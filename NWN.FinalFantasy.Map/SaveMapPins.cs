using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class SaveMapPins: MapPinBase
    {
        public static void Main()
        {
            var player = GetExitingObject();
            if (!GetIsPlayer(player)) return;

            var playerID = GetGlobalID(player);
            var mapPins = DB.GetList<MapPin>(playerID);

            mapPins.Entities.Clear();

            // For every map pin found on a player,
            // create a new entity and store it in the DB.
            for(int x = 0; x < GetNumberOfMapPins(player); x++)
            {
                var mapPinDetails = GetMapPinDetails(player, x);
                if (string.IsNullOrWhiteSpace(mapPinDetails.Text)) continue;

                MapPin mapPin = new MapPin
                {
                    AreaTag = GetTag(mapPinDetails.Area),
                    Text = mapPinDetails.Text,
                    PositionX = mapPinDetails.PositionX,
                    PositionY = mapPinDetails.PositionY
                };

                mapPins.Entities.Add(mapPin);
            }

            DB.Set(mapPins);
        }
    }
}
