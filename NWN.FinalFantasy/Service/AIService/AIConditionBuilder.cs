using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Feature.AIDefinition.AIConditionDefinition;

namespace NWN.FinalFantasy.Service.AIService
{
    public class AIConditionBuilder
    {
        private readonly List<IAICondition> _conditions = new List<IAICondition>();

        /// <summary>
        /// Adds a condition which requires the creature to have one of the specified movement rates.
        /// </summary>
        /// <param name="movementRate">The first movement rate to require.</param>
        /// <param name="additionalMovementRates">Optional, additional movement rates which are also acceptable.</param>
        /// <returns>An AIConditionBuilder</returns>
        public AIConditionBuilder HasMovementRate(MovementRate movementRate, params MovementRate[] additionalMovementRates)
        {
            var movementRates = new List<MovementRate> {movementRate};

            if (additionalMovementRates != null)
            {
                movementRates.AddRange(additionalMovementRates);
            }

            _conditions.Add(new HasMovementRateCondition(movementRates.ToArray()));
            return this;
        }

        /// <summary>
        /// Adds a condition which requires the creature's HP to be at or below the specified percentage.
        /// Percentage can be like so: 0.5 or 50 both translate to 50% or lower of HP.
        /// </summary>
        /// <param name="percentage">The percentage of HP to be at or below.</param>
        /// <returns>An AIConditionBuilder</returns>
        public AIConditionBuilder HPBelowPercentage(float percentage)
        {
            _conditions.Add(new HPPercentageCondition(percentage));

            return this;
        }

        /// <summary>
        /// Adds a condition which requires the creature's MP to be at or below the specified percentage.
        /// Percentage can be like so: 0.5 or 50 both translate to 50% or lower of MP.
        /// </summary>
        /// <param name="percentage">The percentage of MP to be at or below.</param>
        /// <returns>An AIConditionBuilder</returns>
        public AIConditionBuilder MPBelowPercentage(float percentage)
        {
            _conditions.Add(new MPPercentageCondition(percentage));

            return this;
        }

        /// <summary>
        /// Adds a condition which requires the creature's STM to be at or below the specified percentage.
        /// Percentage can be like so: 0.5 or 50 both translate to 50% or lower of STM.
        /// </summary>
        /// <param name="percentage">The percentage of STM to be at or below.</param>
        /// <returns>An AIConditionBuilder</returns>
        public AIConditionBuilder STMBelowPercentage(float percentage)
        {
            _conditions.Add(new STMPercentageCondition(percentage));

            return this;
        }

        public IEnumerable<IAICondition> Build()
        {
            return _conditions;
        }
    }
}
