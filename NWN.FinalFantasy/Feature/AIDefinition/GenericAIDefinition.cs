using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition
{
    public class GenericAIDefinition: IAIListDefinition
    {
        public Dictionary<string, AIInstructionSet> BuildAIs()
        {
            var builder = new AIBuilder();


            return builder.Build();
        }
    }
}
