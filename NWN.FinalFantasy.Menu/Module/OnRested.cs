using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Menu.Module
{
    public class OnRested: IScript
    {
        public void Main()
        {
            NWGameObject player = GetLastPCRested();
            RestEventType restType = GetLastRestEventType();

            if (restType != RestEventType.Started ||
                !GetIsObjectValid(player) ||
                GetIsDungeonMaster(player)) return;

            AssignCommand(player, () => ClearAllActions());
            Conversation.Start(player, player, "Job.Menu.ChangeJobMenu");
        }
    }
}
