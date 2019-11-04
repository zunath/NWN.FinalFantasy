using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class AutoExploreArea
    {
        public static void Main()
        {
            NWGameObject area = NWGameObject.OBJECT_SELF;
            NWGameObject player = GetEnteringObject();

            if (!GetIsPlayer(player)) return;

            if(GetLocalInt(area, "AUTO_EXPLORED") == 1)
            {
                ExploreAreaForPlayer(area, player);
            }
        }
    }
}
