using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Skill = NWN.FinalFantasy.Service.Skill;

namespace NWN.FinalFantasy.Feature
{
    public static class DebuggingTools
    {
        [NWNEventHandler("test1")]
        public static void DebugGiveQuest()
        {
            var player = GetLastUsedBy();
            Quest.AcceptQuest(player, "testQuest");
        }

        [NWNEventHandler("test2")]
        public static void DebugSpawnCreature()
        {
            var location = GetLocation(GetWaypointByTag("DEATH_DEFAULT_RESPAWN_POINT"));
            var spawn = CreateObject(ObjectType.Creature, "test_zombie", location);

            SetLocalInt(spawn, "QUEST_NPC_GROUP_ID", 1);
        }

        [NWNEventHandler("test4")]
        public static void DebugGiveXP()
        {
            var player = GetLastUsedBy();
            Skill.GiveSkillXP(player, SkillType.Longsword, 5000);
        }

        [NWNEventHandler("test6")]
        public static void IncreaseEnmityOnBoy()
        {
            var player = GetLastUsedBy();
            var boy = GetObjectByTag("ENMITY_TARGET");
            var lastAttacker = GetLastAttacker(player);

            Enmity.ModifyEnmity(boy, lastAttacker, 999);
        }

        [NWNEventHandler("test7")]
        public static void GiveEffect()
        {
            var player = GetLastUsedBy();
            StatusEffect.Apply(player, player, StatusEffectType.Invincible, 30.0f);
        }

        [NWNEventHandler("test8")]
        public static void MakeIP()
        {
            Console.WriteLine("firing");

            var itemprop = ItemPropertyAttackBonus(1);

            Console.WriteLine("Unpacking");
            var unpacked = Core.NWNX.ItemProperty.UnpackIP(itemprop);

            Console.WriteLine("Packing");
            var packed = Core.NWNX.ItemProperty.PackIP(unpacked);

            Console.WriteLine("Done");
        }
    }
}
