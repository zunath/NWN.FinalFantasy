namespace NWN.FinalFantasy.Persistence.PlayerLocation
{
    public class SaveOnAreaEnter: SavePlayerLocation
    {
        public static void Main()
        {
            var player = _.GetEnteringObject();
            Run(player);
        }
    }
}
