using static NWN._;

namespace NWN.FinalFantasy.Item.PreventLoginEquipEvents
{
    internal class MarkPlayerLoggingIn
    {
        public static void Main()
        {
            NWGameObject player = GetEnteringObject();
            if (!GetIsPlayer(player)) return;

            SetLocalInt(player, "PLAYER_LOGGING_IN", 1);
        }
    }
}
