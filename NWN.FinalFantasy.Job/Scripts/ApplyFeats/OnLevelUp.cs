using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.ApplyFeats
{
    public class OnLevelUp : ApplyFeatsBase, IScript
    {
        public void Main()
        {
            var data = Script.GetScriptData<LeveledUp>();
            var player = data.Player;
            Apply(player);
        }
    }
}