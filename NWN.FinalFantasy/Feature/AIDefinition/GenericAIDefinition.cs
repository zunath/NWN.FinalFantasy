using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition
{
    public class GenericAIDefinition: IAIListDefinition
    {
        public Dictionary<string, AIInstructionSet> BuildAIs()
        {
            var builder = new AIBuilder()
                .CreateInstructionSet("Generic")
                .RandomWalk(condition =>
                {
                    condition.HasMovementRate(
                        MovementRate.DMFast,
                        MovementRate.Default,
                        MovementRate.Fast,
                        MovementRate.Normal,
                        MovementRate.PC,
                        MovementRate.Slow,
                        MovementRate.VeryFast,
                        MovementRate.VerySlow);
                })
                .PlayAnimation(Animation.LoopingSitCross);


            return builder.Build();
        }
    }
}
