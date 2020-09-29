using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AIInstruction
    {
        public IAITargets Target { get; set; }
        public IAIAction Action { get; set; }
    }

    public class AIInstructionSet: HashSet<AIInstruction>
    {

    }
}
