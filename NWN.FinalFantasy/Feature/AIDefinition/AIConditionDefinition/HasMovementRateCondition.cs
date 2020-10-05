using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Feature.AIDefinition.AIData;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition
{
    public class HasMovementRateCondition: IAICondition
    {
        private readonly MovementRate[] _movementRates;

        public HasMovementRateCondition(params MovementRate[] movementRates)
        {
            _movementRates = movementRates;
        }

        public bool MeetsCondition(uint creature)
        {
            var data = AICreatureData.GetCreature(creature);
            return _movementRates.Contains(data.MovementRate);
        }
    }
}
