using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.Traits
{
    /// <summary>
    /// Applies a percent increase to a player's HP if they have an HP Boost feat.
    /// Only the highest tier of the ability is used for this calculation. They do not stack.
    /// </summary>
    public class ApplyHPBoostTrait: StatProcessingBase, IScript
    {
        public void Main()
        {
            var data = Script.GetScriptData<StatsRecalculated>();
            var player = data.Player;

            float percentIncrease;

            if (_.GetHasFeat(Feat.HPBoost3, player))
            {
                percentIncrease = 0.30f;
            }
            else if (_.GetHasFeat(Feat.HPBoost2, player))
            {
                percentIncrease = 0.20f;
            }
            else if (_.GetHasFeat(Feat.HPBoost1, player))
            {
                percentIncrease = 0.10f;
            }
            else return;

            var hp = _.GetMaxHitPoints(player);
            hp += (int)(hp * percentIncrease);
            ApplyHP(player, hp);
        }
    }
}
