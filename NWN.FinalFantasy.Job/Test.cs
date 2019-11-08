using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Job
{
    internal class Test
    {
        public static void Main()
        {
            var player = _.GetLastUsedBy();
            NWNXCreature.AddFeat(player, Feat.OpenRestMenu);

            Script.Run<JobType>(player, "Job.ChangeJob", JobType.Bard);
        }
    }
}
