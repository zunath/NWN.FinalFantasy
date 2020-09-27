using System;
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
        public static void SimulateTripleTriad()
        {
            var player1 = GetFirstPC();
            var player2 = GetNextPC();

            // Single player mode
            if (!GetIsObjectValid(player2))
            {
                var player = GetLastUsedBy();
                var deck1 = TripleTriad.BuildRandomDeck(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
                var deck2 = TripleTriad.BuildRandomDeck(10, 9, 8, 7);

                TripleTriad.StartGame(player, deck1, player, deck2);
            }

            // Two player mode
            else
            {
                var deck1 = TripleTriad.BuildRandomDeck(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
                var deck2 = TripleTriad.BuildRandomDeck(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                TripleTriad.StartGame(player1, deck1, player2, deck2);
            }
        }

        [NWNEventHandler("test12")]
        public static void UseCard()
        {
            var player = GetLastUsedBy();
            if (!GetIsPC(player) || GetIsDM(player)) return;

            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();

            foreach (var (cardType, card) in TripleTriad.GetAllAvailableCards())
            {
                dbPlayerTripleTriad.AvailableCards[cardType] = DateTime.UtcNow;
            }

            DB.Set(playerId, dbPlayerTripleTriad);

            SendMessageToPC(player, "All Triple Triad cards received! OMGZ");
        }

    }
}
