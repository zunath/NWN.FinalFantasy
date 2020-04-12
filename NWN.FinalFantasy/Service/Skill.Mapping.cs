using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service
{
    public static partial class Skill
    {
        private static readonly Dictionary<BaseItem, SkillType> _itemToSkillMapping = new Dictionary<BaseItem, SkillType>();

        /// <summary>
        /// Handles creating all of the mapping dictionaries used by the skill system on module load.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void LoadMappings()
        {
            LoadItemToSkillMapping();
        }

        /// <summary>
        /// Loads the base item -> skill type mappings.
        /// </summary>
        private static void LoadItemToSkillMapping()
        {
            Console.WriteLine("Loading item to skill mappings.");
            _itemToSkillMapping[BaseItem.Longsword] = SkillType.Longsword;
            _itemToSkillMapping[BaseItem.Gloves] = SkillType.Knuckles;
            _itemToSkillMapping[BaseItem.Dagger] = SkillType.Dagger;
            _itemToSkillMapping[BaseItem.QuarterStaff] = SkillType.Staff;
            _itemToSkillMapping[BaseItem.LightMace] = SkillType.Rod;
            _itemToSkillMapping[BaseItem.Longbow] = SkillType.Longbow;
            Console.WriteLine("Completed item to skill mappings successfully.");
        }

        /// <summary>
        /// Retrieves the skill type associated with a base item type.
        /// If no skill is associated with the item, SkillType.Unknown will be returned.
        /// </summary>
        /// <param name="baseItem">The type of base item to look for.</param>
        /// <returns>A skill type associated with the given base item type.</returns>
        public static SkillType GetSkillTypeByBaseItem(BaseItem baseItem)
        {
            if (!_itemToSkillMapping.ContainsKey(baseItem))
                return SkillType.Unknown;

            return _itemToSkillMapping[baseItem];
        }
    }
}
