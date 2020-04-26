using NWN.FinalFantasy.Core;
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
        public static void DebugSpawnGoblin()
        {
            var location = GetLocation(GetWaypointByTag("DEATH_DEFAULT_RESPAWN_POINT"));
            var spawn = CreateObject(ObjectType.Creature, "nw_goblina", location);

            SetLocalInt(spawn, "QUEST_NPC_GROUP_ID", 1);
        }

        [NWNEventHandler("test3")]
        public static void DebugGiveAchievement()
        {
            var player = GetLastUsedBy();
            Achievement.GiveAchievement(player, AchievementType.TestAchievement);
        }

        [NWNEventHandler("test4")]
        public static void DebugGiveXP()
        {
            var player = GetLastUsedBy();
            Skill.GiveSkillXP(player, SkillType.Longsword, 5000);
        }
    }
}
