using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAIListDefinition
    {
        public Dictionary<string, AIInstructionSet> BuildAIs();
    }
}
