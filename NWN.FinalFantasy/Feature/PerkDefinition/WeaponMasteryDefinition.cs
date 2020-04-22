using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.PerkService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class WeaponMasteryDefinition : IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            LongswordMastery(builder);
            KnucklesMastery(builder);
            DaggerMastery(builder);
            StaffMastery(builder);
            RodMastery(builder);
            LongbowMastery(builder);
            KatanaMastery(builder);

            return builder.Build();
        }

        private static void ModifyBAB(uint player, uint item, InventorySlot inventorySlot, int level, bool isApplying, BaseItem requiredItemType, InventorySlot requiredSlot)
        {
            var baseItemType = GetBaseItemType(item);
            if (baseItemType != requiredItemType) return;
            if (inventorySlot != requiredSlot) return;

            var amount = isApplying ? level : -level;
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            Stat.AdjustBAB(dbPlayer, player, amount);
            DB.Set(playerId, dbPlayer);
        }

        private static void LongswordMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.LongswordMastery)
                .Name("Longsword Mastery")
                .Description("Grants increased BAB when equipped with a longsword.")
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    ModifyBAB(player, item, inventorySlot, level, true, BaseItem.Longsword, InventorySlot.RightHand);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    ModifyBAB(player, item, InventorySlot.Invalid, level, false, BaseItem.Longsword, InventorySlot.Invalid);
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with a longsword.")
                .Price(8)
                .RequirementSkill(SkillType.Longsword, 30)
                .RequirementSkill(SkillType.Chivalry, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with a longsword.")
                .Price(8)
                .RequirementSkill(SkillType.Longsword, 60)
                .RequirementSkill(SkillType.Chivalry, 60)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with a longsword.")
                .Price(8)
                .RequirementSkill(SkillType.Longsword, 90)
                .RequirementSkill(SkillType.Chivalry, 75);
        }

        private static void KnucklesMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Monk, PerkType.KnucklesMastery)
                .Name("Knuckles Mastery")
                .Description("Grants increased BAB when equipped with knuckles.")

                // Knuckles BAB calculations work a little backwards compared to the other item types.
                // We want to add BAB when the player is newly bare-handed.
                // We want to remove BAB when the player equips any item into their right or left hands.
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    // There are items in the right or left hands right now. Exit early
                    if (GetIsObjectValid(GetItemInSlot(InventorySlot.RightHand, player)) ||
                        GetIsObjectValid(GetItemInSlot(InventorySlot.LeftHand, player))) return;

                    // Nothing was equipped, but we're about to put an item in either hand.
                    // For this scenario, we need to reduce the BAB because the player is about to have a weapon equipped.
                    if(inventorySlot == InventorySlot.RightHand || inventorySlot == InventorySlot.LeftHand)
                        ModifyBAB(player, item, InventorySlot.Invalid, level, false, GetBaseItemType(item), InventorySlot.Invalid);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    var rightHand = GetItemInSlot(InventorySlot.RightHand, player);
                    var leftHand = GetItemInSlot(InventorySlot.LeftHand, player);
                    var rightValid = GetIsObjectValid(rightHand);
                    var leftValid = GetIsObjectValid(leftHand);

                    // The item being removed is in either the right or left hand and the OTHER hand is empty.
                    // We need to apply the BAB bonus.
                    if ((rightHand == item && !leftValid) ||
                        (leftHand == item && !rightValid))
                    {
                        ModifyBAB(player, item, InventorySlot.Invalid, level, true, GetBaseItemType(item), InventorySlot.Invalid);
                    }
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with knuckles.")
                .Price(8)
                .RequirementSkill(SkillType.Knuckles, 30)
                .RequirementSkill(SkillType.Chi, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with knuckles.")
                .Price(8)
                .RequirementSkill(SkillType.Knuckles, 60)
                .RequirementSkill(SkillType.Chi, 50)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with knuckles.")
                .Price(8)
                .RequirementSkill(SkillType.Knuckles, 90)
                .RequirementSkill(SkillType.Chi, 75);
        }


        private static void DaggerMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Thief, PerkType.DaggerMastery)
                .Name("Dagger Mastery")
                .Description("Grants increased BAB when equipped with a dagger.")
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    ModifyBAB(player, item, inventorySlot, level, true, BaseItem.Dagger, InventorySlot.RightHand);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    ModifyBAB(player, item, InventorySlot.Invalid, level, false, BaseItem.Dagger, InventorySlot.Invalid);
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with a dagger.")
                .Price(8)
                .RequirementSkill(SkillType.Dagger, 30)
                .RequirementSkill(SkillType.Thievery, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with a dagger.")
                .Price(8)
                .RequirementSkill(SkillType.Dagger, 60)
                .RequirementSkill(SkillType.Thievery, 50)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with a dagger.")
                .Price(8)
                .RequirementSkill(SkillType.Dagger, 90)
                .RequirementSkill(SkillType.Thievery, 75);
        }


        private static void StaffMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.BlackMage, PerkType.StaffMastery)
                .Name("Staff Mastery")
                .Description("Grants increased BAB when equipped with a staff.")
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    ModifyBAB(player, item, inventorySlot, level, true, BaseItem.QuarterStaff, InventorySlot.RightHand);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    ModifyBAB(player, item, InventorySlot.Invalid, level, false, BaseItem.QuarterStaff, InventorySlot.Invalid);
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with a staff.")
                .Price(8)
                .RequirementSkill(SkillType.Staff, 30)
                .RequirementSkill(SkillType.BlackMagic, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with a staff.")
                .Price(8)
                .RequirementSkill(SkillType.Staff, 60)
                .RequirementSkill(SkillType.BlackMagic, 50)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with a staff.")
                .Price(8)
                .RequirementSkill(SkillType.Staff, 90)
                .RequirementSkill(SkillType.BlackMagic, 75);
        }


        private static void RodMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.WhiteMage, PerkType.RodMastery)
                .Name("Rod Mastery")
                .Description("Grants increased BAB when equipped with a rod.")
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    ModifyBAB(player, item, inventorySlot, level, true, BaseItem.LightMace, InventorySlot.RightHand);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    ModifyBAB(player, item, InventorySlot.Invalid, level, false, BaseItem.LightMace, InventorySlot.Invalid);
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with a rod.")
                .Price(8)
                .RequirementSkill(SkillType.Rod, 30)
                .RequirementSkill(SkillType.WhiteMagic, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with a rod.")
                .Price(8)
                .RequirementSkill(SkillType.Rod, 60)
                .RequirementSkill(SkillType.WhiteMagic, 50)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with a rod.")
                .Price(8)
                .RequirementSkill(SkillType.Rod, 90)
                .RequirementSkill(SkillType.WhiteMagic, 75);
        }


        private static void LongbowMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Ranger, PerkType.LongbowMastery)
                .Name("Longbow Mastery")
                .Description("Grants increased BAB when equipped with a longbow.")
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    ModifyBAB(player, item, inventorySlot, level, true, BaseItem.Longbow, InventorySlot.RightHand);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    ModifyBAB(player, item, InventorySlot.Invalid, level, false, BaseItem.Longbow, InventorySlot.Invalid);
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with a longbow.")
                .Price(8)
                .RequirementSkill(SkillType.Longbow, 30)
                .RequirementSkill(SkillType.Archery, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with a longbow.")
                .Price(8)
                .RequirementSkill(SkillType.Longbow, 60)
                .RequirementSkill(SkillType.Archery, 50)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with a longbow.")
                .Price(8)
                .RequirementSkill(SkillType.Longbow, 90)
                .RequirementSkill(SkillType.Archery, 75);
        }

        private static void KatanaMastery(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Ninja, PerkType.KatanaMastery)
                .Name("Katana Mastery")
                .Description("Grants increased BAB when equipped with a katana.")
                .TriggerEquippedItem((player, item, inventorySlot, type, level) =>
                {
                    ModifyBAB(player, item, inventorySlot, level, true, BaseItem.Katana, InventorySlot.RightHand);
                })
                .TriggerUnequippedItem((player, item, type, level) =>
                {
                    ModifyBAB(player, item, InventorySlot.Invalid, level, false, BaseItem.Katana, InventorySlot.Invalid);
                })

                .AddPerkLevel(1)
                .Description("Grants +1 BAB when equipped with a katana.")
                .Price(8)
                .RequirementSkill(SkillType.Katana, 30)
                .RequirementSkill(SkillType.Ninjitsu, 25)

                .AddPerkLevel(2)
                .Description("Grants +2 BAB when equipped with a katana.")
                .Price(8)
                .RequirementSkill(SkillType.Katana, 60)
                .RequirementSkill(SkillType.Ninjitsu, 50)

                .AddPerkLevel(3)
                .Description("Grants +3 BAB when equipped with a katana.")
                .Price(8)
                .RequirementSkill(SkillType.Katana, 90)
                .RequirementSkill(SkillType.Ninjitsu, 75);
        }
    }
}
