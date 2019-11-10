using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Plays a laughing animation.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Laughing : LoopingAnimationCommand
    {
        protected override void DoAction(NWGameObject user, float duration)
        {
            _.AssignCommand(user, () => _.ActionPlayAnimation(AnimationLooping.Talk_Laughing));
        }
    }
}