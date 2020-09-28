using System;
using System.Threading;
using System.Threading.Tasks;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Feature.DialogDefinition;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Dialog = NWN.FinalFantasy.Service.Dialog;
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

        [NWNEventHandler("test9")]
        public static void OpenHomePurchaseMenu()
        {
            var player = GetLastUsedBy();

            Creature.AddFeatByLevel(player, Feat.PropertyTool, 1);

            Dialog.StartConversation(player, OBJECT_SELF, nameof(PlayerHouseDialog));
        }

        [NWNEventHandler("test10")]
        public static void SpawnGold()
        {
            var player = GetLastUsedBy();
            GiveGoldToCreature(player, 5000);
        }

        [NWNEventHandler("test11")]
        public static async void ThreadProcessing()
        {
            var id = Guid.NewGuid().ToString();
            Console.WriteLine($"thread processing started. ID = {id}");
            Console.WriteLine($"Thread = {Thread.CurrentThread.ManagedThreadId}");
            var task1 = Task.Run(() =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"Hello from thread: {threadId}");

                Thread.Sleep(10000);
                Console.WriteLine($"stage 1 thread done thread id = {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(5000);
                Console.WriteLine($"stage 2 thread done thread id = {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"stage 3 thread done thread id = {Thread.CurrentThread.ManagedThreadId}");
            });

            await NWTask.WhenAll(task1);

            Console.WriteLine($"thread processing done. ID = {id}");
        }

    }
}
