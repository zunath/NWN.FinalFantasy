using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Job.Event;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    /// <summary>
    /// Grants the mastered feat to the player.
    /// </summary>
    public class MasterAbility: IScript
    {
        public void Main()
        {
            var data = Script.GetScriptData<AbilityMastered>();
            var player = data.Player;
            var feat = data.AbilityLearned;
            var ability = AbilityRegistry.Get(feat);

            NWNXCreature.AddFeatByLevel(player, feat, 1);

            SendMessageToPC(player, $"Ability '{ability.Name}' MASTERED!");
        }
    }
}
