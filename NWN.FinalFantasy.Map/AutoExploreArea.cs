using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class AutoExploreArea: IScript
    {
        public void Main()
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
