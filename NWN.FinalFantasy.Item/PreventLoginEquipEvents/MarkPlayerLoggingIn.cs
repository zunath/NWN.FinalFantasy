using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Item.PreventLoginEquipEvents
{
    public class MarkPlayerLoggingIn: IScript
    {
        public void Main()
        {
            NWGameObject player = GetEnteringObject();
            if (!GetIsPlayer(player)) return;

            SetLocalInt(player, "PLAYER_LOGGING_IN", 1);
        }
    }
}
