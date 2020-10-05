using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition;
using NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition;

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

        public AIBuilder AddInstruction(IAIAction action, IAITargets targets, Action<AIConditionBuilder> conditions = null)
        {
            var conditionBuilder = new AIConditionBuilder();
            conditions?.Invoke(conditionBuilder);
            var builtConditions = conditionBuilder.Build();

            _currentInstructionSet.Add(new AIInstruction
            {
                Action = new Tuple<IEnumerable<IAICondition>, IAIAction>(builtConditions, action),
                Targets = targets
            });

            return this;
        }

        public AIBuilder RandomWalk(Action<AIConditionBuilder> conditions = null)
        {
            AddInstruction(new RandomWalkAction(), new SelfTarget(), conditions);
            return this;
        }

        public AIBuilder Attack(IAITargets targets, Action<AIConditionBuilder> conditions = null)
        {
            AddInstruction(new AttackAction(), targets, conditions);
            return this;
        }

        public AIBuilder PlayAnimation(Animation animation, float duration = 1f, Action<AIConditionBuilder> conditions = null)
        {
            AddInstruction(new AnimationAction(animation, duration), new SelfTarget(), conditions);
            return this;
        }

        public Dictionary<string, AIInstructionSet> Build()
        {
            return _instructionSets;
        }
    }
}
