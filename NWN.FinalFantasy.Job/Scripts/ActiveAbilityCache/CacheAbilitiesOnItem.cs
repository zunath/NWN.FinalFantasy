using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts.ActiveAbilityCache
{
    /// <summary>
    /// Checks a single equipped item for all of the abilities attached to it.
    /// The quantity of each ability equipped is cached on the player for later retrieval
    /// when granting AP and marking mastery of an ability.
    /// </summary>
    internal class CacheAbilitiesOnItem
    {
        public static void Main()
        {
            var player = GetPCItemLastEquippedBy();
            if (!GetIsPlayer(player)) return;

            var playerID = GetGlobalID(player);
            var item = GetPCItemLastEquipped();
            var equippedAbilities = EquippedAbilityRepo.Get(playerID);

            var ip = GetFirstItemProperty(item);
            while(GetIsItemPropertyValid(ip))
            {
                if (GetItemPropertyType(ip) == ItemPropertyType.Ability)
                {
                    // The IPRP_FFOAbility.2da file IDs should link up one-to-one with the Feat.2da file.
                    // Therefore, this conversion should work.
                    var feat = (Feat)GetItemPropertySubType(ip);
                    var ability = equippedAbilities.Entities.SingleOrDefault(x => x.Feat == feat);

                    if (ability == null)
                    {
                        ability = new EquippedAbility
                        {
                            Feat = feat
                        };
                        equippedAbilities.Entities.Add(ability);
                    }

                    ability.Quantity++;
                }

                ip = GetNextItemProperty(item);
            }

            EquippedAbilityRepo.Set(playerID, equippedAbilities);
        }
    }
}
