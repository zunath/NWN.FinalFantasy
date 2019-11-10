using System.Collections.Generic;
using NWN.FinalFantasy.Job.Enumeration;
using NWN.FinalFantasy.Job.StatRating;

namespace NWN.FinalFantasy.Job.Registry
{
    internal class RatingRegistry
    {
        private static readonly Dictionary<ProficiencyRating, RatingChart> _ratingCharts = new Dictionary<ProficiencyRating, RatingChart>();

        public static void Register()
        {
            _ratingCharts[ProficiencyRating.A] = new RatingA();
            _ratingCharts[ProficiencyRating.B] = new RatingB();
            _ratingCharts[ProficiencyRating.C] = new RatingC();
            _ratingCharts[ProficiencyRating.D] = new RatingD();
            _ratingCharts[ProficiencyRating.E] = new RatingE();
        }

        public static RatingChart Get(ProficiencyRating rating)
        {
            return _ratingCharts[rating];
        }
    }
}
