using NWN.FinalFantasy.Quest.TestLocation;

namespace NWN.FinalFantasy.Quest
{
    public class Startup
    {
        /// <summary>
        /// This script should be used to initialize all quests into the registry.
        /// </summary>
        public static void Main()
        {
            QuestRegistry.Register(new MyTestQuest());
        }
    }
}
