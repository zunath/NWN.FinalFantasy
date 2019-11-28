using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Item.PreventCombatItemSwaps
{
    public abstract class PreventCombatItemSwapsBase
    {
        protected static void Prevent()
        {
            NWGameObject player = NWGameObject.OBJECT_SELF;
            if (!GetIsPlayer(player)) return;

            if (GetIsInCombat(player))
            {
                SendMessageToPC(player, "Equipment cannot be modified during combat.");
                NWNXEvents.SkipEvent();
            }
        }
    }
}
