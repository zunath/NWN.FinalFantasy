using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Job
{
    public class TestBread: IScript
    {
        public void Main()
        {
            var player = GetLastUsedBy();
            int xp = 500;
            GiveJobXP(player, xp);


            var wp = GetObjectByTag("ooc_wp");
            var location = GetLocation(wp);
            CreateObject(ObjectType.Creature, "nw_malekid01", location);
        }
    }
}
