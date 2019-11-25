using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Location
{
    public class SaveOnModuleExit: SavePlayerLocation, IScript
    {
        public void Main()
        {
            var player = _.GetExitingObject();
            Run(player);
        }
    }
}
