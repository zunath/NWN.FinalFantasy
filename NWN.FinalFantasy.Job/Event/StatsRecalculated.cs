namespace NWN.FinalFantasy.Job.Event
{
    internal class StatsRecalculated
    {
        public NWGameObject Player { get; set; }

        public StatsRecalculated(NWGameObject player)
        {
            Player = player;
        }
    }
}
