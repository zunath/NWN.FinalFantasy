using System;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Item.PreventLoginEquipEvents
{
    public class PreventEquipEventOnLogin: IScript
    {
        public void Main()
        {
            var player = NWGameObject.OBJECT_SELF;
            if (!GetIsPlayer(player)) return;

            if(GetLocalInt(player, "PLAYER_LOGGING_IN") == 1)
            {
                Console.WriteLine("Skipping equip event on login");
                NWNXEvents.SkipEvent();
            }
        }
    }
}
