using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public static class RestMenu
    {
        [NWNEventHandler("mod_rest")]
        public static void OpenRestMenu()
        {
            var player = GetLastPCRested();
            var restType = GetLastRestEventType();

            if (restType != RestEventType.Started ||
                !GetIsObjectValid(player) ||
                GetIsDM(player)) return;

            AssignCommand(player, () => ClearAllActions());

            Dialog.StartConversation(player, player, "RestMenuDialog");
        }
    }
}
