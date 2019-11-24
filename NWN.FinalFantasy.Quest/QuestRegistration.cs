using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Quest.TestLocation;
using Serilog;

namespace NWN.FinalFantasy.Quest
{
    public class QuestRegistration: IFeatureRegistration
    {
        /// <summary>
        /// This script should be used to initialize all quests into the registry.
        /// </summary>
        public void Register()
        {
            Log.Information("Registering Quest...");
            QuestRegistry.Register(new MyTestQuest());
        }
    }
}
