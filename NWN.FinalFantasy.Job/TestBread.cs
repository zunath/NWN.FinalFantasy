namespace NWN.FinalFantasy.Job
{
    internal class TestBread
    {
        public static void Main()
        {
            var player = _.GetLastUsedBy();
            int xp = 500;
            _.GiveJobXP(player, xp);
        }
    }
}
