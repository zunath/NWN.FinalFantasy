namespace NWN.FinalFantasy.Location
{
    public class SaveOnModuleExit: SavePlayerLocation
    {
        public static void Main()
        {
            var player = _.GetExitingObject();
            Run(player);
        }
    }
}
