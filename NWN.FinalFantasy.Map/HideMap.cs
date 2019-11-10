using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class HideMap
    {
        public static void Main()
        {
            NWGameObject area = NWGameObject.OBJECT_SELF;

            if (GetLocalInt(area, "HIDE_MINIMAP") == 0) return;

            var player = GetFirstPC();
            while (GetIsObjectValid(player))
            {
                if (GetArea(player) == area)
                {
                    ExploreAreaForPlayer(area, player, false);
                }

                player = GetNextPC();
            }
        }
    }
}
