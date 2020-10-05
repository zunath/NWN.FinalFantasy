using NWN.FinalFantasy.Feature.AIDefinition.AIData;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition
{
    public class MPPercentageCondition : IAICondition
    {
        private readonly float _mpThreshold;

        public MPPercentageCondition(float mpThreshold)
        {
            if (mpThreshold > 1f)
            {
                _mpThreshold = mpThreshold * 0.01f;
            }
            else
            {
                _mpThreshold = mpThreshold;
            }
        }

        public bool MeetsCondition(uint creature)
        {
            var creatureData = AICreatureData.GetCreature(creature);
            var meetsCondition = (float)creatureData.CurrentMP / (float)creatureData.MaxMP <= _mpThreshold;

            return meetsCondition;
        }
    }
}