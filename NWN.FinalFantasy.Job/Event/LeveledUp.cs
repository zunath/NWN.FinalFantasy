namespace NWN.FinalFantasy.Job.Event
{
    public class LeveledUp
    {
        public NWGameObject Player { get; set; }

        public LeveledUp(NWGameObject player)
        {
            Player = player;
        }
    }
}
