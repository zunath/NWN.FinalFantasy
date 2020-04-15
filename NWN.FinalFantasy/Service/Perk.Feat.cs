using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service
{
    public static partial class Perk
    {
        private static readonly Dictionary<Feat, PerkType> _featsToPerks = new Dictionary<Feat, PerkType>();

        /// <summary>
        /// When the module loads, feats will be linked to perk types.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void CacheFeatsToPerks()
        {

        }

        /// <summary>
        /// Returns true if a feat is registered to any perk.
        /// Returns false otherwise.
        /// </summary>
        /// <param name="feat">The feat to check.</param>
        /// <returns>true if registered, false otherwise</returns>
        public static bool IsFeatRegisteredToPerk(Feat feat)
        {
            return _featsToPerks.ContainsKey(feat);
        }

        /// <summary>
        /// Retrieves a perk type which is mapped to the specified feat.
        /// If feat is not mapped, an exception will be raised.
        /// </summary>
        /// <param name="feat">The feat to search for.</param>
        /// <returns>The PerkType associated with a feat.</returns>
        public static PerkType GetPerkByFeat(Feat feat)
        {
            if(!_featsToPerks.ContainsKey(feat))
                throw new KeyNotFoundException($"Feat {feat} has not been registered in the Perk.Feat.cs file. It must be registered first.");

            return _featsToPerks[feat];
        }


    }
}
