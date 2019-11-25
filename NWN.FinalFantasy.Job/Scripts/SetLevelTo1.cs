using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    /// <summary>
    /// This is a workaround to prevent the server from crashing when a player leaves the server.
    /// </summary>
    public class SetLevelTo1: IScript
    {
        public void Main()
        {
            NWGameObject player = NWGameObject.OBJECT_SELF;
            if (!GetIsPlayer(player)) return;

            NWNXCreature.SetLevelByPosition(player, ClassPosition.First, 1);
        }
    }
}
