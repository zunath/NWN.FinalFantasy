using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Event;

namespace NWN.FinalFantasy.Job.Scripts.Traits
{
    /// <summary>
    /// Handles adding bonus AC to players with the Defense Bonus feats.
    /// </summary>
    public class ApplyDefenseBonusTrait: StatProcessingBase, IScript
    {
        public void Main()
        {
            var data = Script.GetScriptData<StatsRecalculated>();
            var player = data.Player;
            int baseAC = NWNXCreature.GetBaseAC(player);
            int increaseBy;

            if (_.GetHasFeat(Feat.DefenseBonus1, player))
            {
                increaseBy = 3;
            }
            else
            {
                return;
            }

            baseAC += increaseBy;
            NWNXCreature.SetBaseAC(player, baseAC);
        }
    }
}
