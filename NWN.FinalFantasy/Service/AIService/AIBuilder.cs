using System.Collections.Generic;
using NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition;
using NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AIBuilder
    {
        public Dictionary<string, AIInstructionSet> Build()
        {
            return new Dictionary<string, AIInstructionSet>
            {
                { "Generic", new AIInstructionSet
                {
                    new AIInstruction
                    {
                        Target = new NearestTarget(),
                        Action = new AttackAction()
                    }
                }}
            }; // todo implement
        }
    }
}
