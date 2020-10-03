using NWN.FinalFantasy.Feature.AIDefinition.AIData;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition
{
    public class HPCondition: IAICondition
    {
        private readonly float _hpThreshold;

        public HPCondition(float hpThreshold)
        {
            if (hpThreshold > 1f)
            {
                _hpThreshold = hpThreshold * 0.01f;
            }
            else
            {
                _hpThreshold = hpThreshold;
            }
        }

        public bool MeetsCondition(uint creature)
        {
            var creatureData = AICreatureData.GetCreature(creature);
            var meetsCondition = (float)creatureData.CurrentHP / (float)creatureData.MaxHP >= _hpThreshold;

            return meetsCondition;
        }
    }
}
