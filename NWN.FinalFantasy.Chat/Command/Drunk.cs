using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Plays a drunk animation.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Drunk : LoopingAnimationCommand
    {
        protected override void DoAction(NWGameObject user, float duration)
        {
            _.AssignCommand(user, () => _.ActionPlayAnimation(AnimationLooping.Pause_Drunk));
        }
    }
}