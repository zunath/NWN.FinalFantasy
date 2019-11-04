using System;

namespace NWN.FinalFantasy.Map
{
    public abstract class MapProgressionBase
    {
        protected static string BuildKey(Guid playerID, string areaResref)
        {
            return $"MapProgression:{playerID}:{areaResref}";
        }
    }
}
