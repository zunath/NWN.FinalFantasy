using System;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Event;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts.Traits
{
    public class ApplyWeaponMasteryTrait: IScript
    {
        private static readonly BaseItemType[] _axeTypes =
        {
            BaseItemType.Battleaxe,
            BaseItemType.DoubleAxe,
            BaseItemType.GreatAxe,
            BaseItemType.HandAxe,
            BaseItemType.ThrowingAxe
        };

        public void Main()
        {
            var data = Script.GetScriptData<StatsRecalculated>();
            var player = data.Player;

            var main = GetItemInSlot(InventorySlot.RightHand, player);
            var off = GetItemInSlot(InventorySlot.LeftHand, player);
            var mainType = GetBaseItemType(main);
            var offType = GetBaseItemType(off);

            int mainBonus = 0;
            int offBonus = 0;

            if (GetLocalInt(main, "EXCLUDE_FROM_CALCULATIONS") == 0)
                mainBonus = GetFeatBonusBAB(player, mainType);

            if(GetLocalInt(off, "EXCLUDE_FROM_CALCULATIONS") == 0)
                offBonus = GetFeatBonusBAB(player, offType);

            int applyBonus = mainBonus;

            if (offBonus > mainBonus)
                applyBonus = offBonus;

            if (applyBonus > 0)
            {
                var bab = GetBaseAttackBonus(player) + applyBonus;
                NWNXCreature.SetBaseAttackBonus(player, bab);
            }
        }

        private static int GetFeatBonusBAB(NWGameObject player, BaseItemType type)
        {
            // Axe Mastery
            if (_axeTypes.Contains(type))
            {
                if (GetHasFeat(Feat.AxeMastery2, player))
                    return 2;
                if (GetHasFeat(Feat.AxeMastery1, player))
                    return 1;

                return 0;
            }

            return 0;
        }

    }
}
