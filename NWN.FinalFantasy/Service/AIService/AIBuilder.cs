using System.Collections.Generic;
using NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition;
using NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition;
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
                        Condition = new HPCondition(50f),
                        Targets = new FirstPlayerTarget(),
                        Action = new AttackAction()
                    }
                }}
            }; // todo implement
        }
    }
}
