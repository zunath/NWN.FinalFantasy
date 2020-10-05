using NWN.FinalFantasy.Feature.AIDefinition.AIData;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition
{
    public class STMPercentageCondition : IAICondition
    {
        private readonly float _stmThreshold;

        public STMPercentageCondition(float stmThreshold)
        {
            if (stmThreshold > 1f)
            {
                _stmThreshold = stmThreshold * 0.01f;
            }
            else
            {
                _stmThreshold = stmThreshold;
            }
        }

        public bool MeetsCondition(uint creature)
        {
            var creatureData = AICreatureData.GetCreature(creature);
            var meetsCondition = (float)creatureData.CurrentSTM / (float)creatureData.MaxSTM <= _stmThreshold;

            return meetsCondition;
        }
    }
}