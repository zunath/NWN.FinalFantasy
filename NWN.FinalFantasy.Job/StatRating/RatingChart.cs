using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.StatRating
{
    internal abstract class RatingChart
    {
        private readonly Dictionary<RatingStat, Dictionary<int, int>> _ratingChart;
        
        protected RatingChart()
        {
            _ratingChart = new Dictionary<RatingStat, Dictionary<int, int>>();
        }

        /// <summary>
        /// Sets a rating's value for each level.
        /// </summary>
        /// <param name="rating">The rating to set</param>
        /// <param name="values">The value to set</param>
        protected void Set(RatingStat rating, params int[] values)
        {
            if(!_ratingChart.ContainsKey(rating))
                _ratingChart[rating] = new Dictionary<int, int>();

            for(int level = 1; level <= values.Length; level++)
                _ratingChart[rating][level] = values[level-1];
        }

        /// <summary>
        /// Retrieves a value by rating and level.
        /// </summary>
        /// <param name="rating">The rating to retrieve.</param>
        /// <param name="level">The level to retrieve</param>
        /// <returns>The value for a given rating and level.</returns>
        public int Get(RatingStat rating, int level)
        {
            if(!_ratingChart.ContainsKey(rating))
                throw new Exception($"Rating '{rating}' has not been registered.");
            if(!_ratingChart[rating].ContainsKey(level))
                throw new Exception($"Level {level} has not been registered for rating type {rating}");

            return _ratingChart[rating][level];
        }
    }
}
