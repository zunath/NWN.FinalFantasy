using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Location
{
    public class SaveOnAreaEnter: SavePlayerLocation, IScript
    {
        public void Main()
        {
            var player = _.GetEnteringObject();
            Run(player);
        }
    }
}
