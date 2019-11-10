using static NWN._;

namespace NWN.FinalFantasy.Item.PreventLoginEquipEvents
{
    internal class UnmarkPlayerLoggingIn
    {
        public static void Main()
        {
            NWGameObject player = GetEnteringObject();
            if (!GetIsPlayer(player)) return;

            DeleteLocalInt(player, "PLAYER_LOGGING_IN");
        }
    }
}
