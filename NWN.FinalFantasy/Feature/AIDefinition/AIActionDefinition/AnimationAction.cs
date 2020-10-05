using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service.AIService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition
{
    public class AnimationAction: IAIAction
    {
        private readonly Animation _animation;
        private readonly float _duration;

        public AnimationAction(Animation animation, float duration = 1f)
        {
            _animation = animation;
            _duration = duration;
        }

        public void Action(uint creature, params uint[] targets)
        {
            ActionPlayAnimation(_animation, 1f, _duration);
        }
    }
}
