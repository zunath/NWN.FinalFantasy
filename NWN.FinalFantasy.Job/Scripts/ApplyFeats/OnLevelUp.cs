using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.ApplyFeats
{
    internal class OnLevelUp : ApplyFeatsBase
    {
        public static void Main()
        {
            var data = Script.GetScriptData<LeveledUp>();
            var player = data.Player;
            Apply(player);
        }
    }
}