using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Job
{
    public class TestBread: IScript
    {
        public void Main()
        {
            var player = _.GetLastUsedBy();
            int xp = 500;
            _.GiveJobXP(player, xp);
        }
    }
}
