namespace NWN.FinalFantasy.Location
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
