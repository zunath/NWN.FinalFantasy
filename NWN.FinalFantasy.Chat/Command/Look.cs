using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Plays a look far animation.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Look : LoopingAnimationCommand
    {
        protected override void DoAction(NWGameObject user, float duration)
        {
            _.AssignCommand(user, () => _.ActionPlayAnimation(AnimationLooping.Look_Far));
        }
    }
}