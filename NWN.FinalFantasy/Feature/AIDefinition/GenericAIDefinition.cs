using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition;
using NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition;
using NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition
{
    public class GenericAIDefinition: IAIListDefinition
    {
        public Dictionary<string, AIInstructionSet> BuildAIs()
        {
            var builder = new AIBuilder()
                .CreateInstructionSet("Generic")
                .AddAction(new AttackAction(), new FirstPlayerTarget(), new HPCondition(99))
                .AddAction(new SitAction(), new SelfTarget());


            return builder.Build();
        }
    }
}
