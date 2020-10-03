using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AIInstruction
    {
        public IAITargets Targets { get; set; }
        public Tuple<IEnumerable<IAICondition>, IAIAction> Action { get; set; }
    }

    public class AIInstructionSet: HashSet<AIInstruction>
    {

    }
}
