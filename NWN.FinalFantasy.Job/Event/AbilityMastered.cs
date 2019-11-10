using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Job.Event
{
    internal class AbilityMastered
    {
        public NWGameObject Player { get; set; }
        public Feat AbilityLearned { get; set; }

        public AbilityMastered(NWGameObject player, Feat feat)
        {
            Player = player;
            AbilityLearned = feat;
        }
    }
}
