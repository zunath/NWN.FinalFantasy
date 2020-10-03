using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AIInstruction
    {
        public IAICondition Condition { get; set; }
        public IAITargets Targets { get; set; }
        public IAIAction Action { get; set; }
    }

    public class AIInstructionSet: HashSet<AIInstruction>
    {

    }
}
