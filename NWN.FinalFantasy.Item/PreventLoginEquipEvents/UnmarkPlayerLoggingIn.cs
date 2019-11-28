using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Item.PreventLoginEquipEvents
{
    public class UnmarkPlayerLoggingIn: IScript
    {
        public void Main()
        {
            NWGameObject player = GetEnteringObject();
            if (!GetIsPlayer(player)) return;

            DeleteLocalInt(player, "PLAYER_LOGGING_IN");
        }
    }
}
