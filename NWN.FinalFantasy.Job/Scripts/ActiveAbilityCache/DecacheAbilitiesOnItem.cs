using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts.ActiveAbilityCache
{
    /// <summary>
    /// Checks a single unequipped item for all of the abilities attached to it.
    /// Reduces the quantity stored in the cache for this player for each ability.
    /// If the quantity of an ability drops to zero or less, the entry is removed from the list.
    /// </summary>
    internal class DecacheAbilitiesOnItem
    {
        public static void Main()
        {
            var player = GetPCItemLastUnequippedBy();
            if (!GetIsPlayer(player)) return;

            var item = GetPCItemLastUnequipped();
            var playerID = GetGlobalID(player);
            var equippedAbilities = EquippedAbilityRepo.Get(playerID);

            var ip = GetFirstItemProperty(item);
            while(GetIsItemPropertyValid(ip))
            {
                if(GetItemPropertyType(ip) == ItemPropertyType.Ability)
                {
                    // The IPRP_FFOAbility.2da file IDs should link up one-to-one with the Feat.2da file.
                    // Therefore, this conversion should work.
                    var feat = (Feat)GetItemPropertySubType(ip);
                    var ability = equippedAbilities.Entities.Single(x => x.Feat == feat);
                    ability.Quantity--;

                    if (ability.Quantity <= 0)
                        equippedAbilities.Entities.Remove(ability);

                }

                ip = GetNextItemProperty(item);
            }

            EquippedAbilityRepo.Set(playerID, equippedAbilities);
        }
    }
}
