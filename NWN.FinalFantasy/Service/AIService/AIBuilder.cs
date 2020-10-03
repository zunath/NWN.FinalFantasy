using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AIBuilder
    {
        private readonly Dictionary<string, AIInstructionSet> _instructionSets = new Dictionary<string, AIInstructionSet>();
        private AIInstructionSet _currentInstructionSet;

        public AIBuilder CreateInstructionSet(string instructionSetId)
        {
            _currentInstructionSet = new AIInstructionSet();
            _instructionSets[instructionSetId] = _currentInstructionSet;

            return this;
        }

        public AIBuilder AddAction(IAIAction action, IAITargets targets, params IAICondition[] conditions)
        {
            _currentInstructionSet.Add(new AIInstruction
            {
                Action = new Tuple<IEnumerable<IAICondition>, IAIAction>(conditions, action),
                Targets = targets
            });

            return this;
        }

        public Dictionary<string, AIInstructionSet> Build()
        {
            return _instructionSets;
        }
    }
}
