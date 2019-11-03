using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Plays a dead front animation.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class DeadFront : LoopingAnimationCommand
    {
        protected override void DoAction(NWGameObject user, float duration)
        {
            _.AssignCommand(user, () => _.ActionPlayAnimation(AnimationLooping.Dead_Front, 1.0f, duration));
        }
    }
}