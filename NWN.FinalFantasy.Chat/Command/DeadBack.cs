using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Plays a dead back animation.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class DeadBack : LoopingAnimationCommand
    {
        protected override void DoAction(NWGameObject user, float duration)
        {
            _.AssignCommand(user, () => _.ActionPlayAnimation(AnimationLooping.Dead_Back, 1.0f, duration));
        }
    }
}