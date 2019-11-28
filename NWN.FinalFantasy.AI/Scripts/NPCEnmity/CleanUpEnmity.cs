using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWN.FinalFantasy.Core.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.AI.Scripts.NPCEnmity
{
    public class CleanUpEnmity: IScript
    {
        public void Main()
        {
            var self = NWGameObject.OBJECT_SELF;
            var table = Enmity.GetOrCreateEnmityTable(self);
            if (table.Count <= 0) return;

            for (int x = table.Count - 1; x >= 0; x--)
            {
                var enmity = table.ElementAt(x);
                var val = enmity.Value;
                var target = val.Target;

                // Remove invalid objects from the enmity table
                if (target == null ||
                    !GetIsObjectValid(target) ||
                    GetArea(target) != GetArea(self) ||
                    GetCurrentHitPoints(target) <= -11 ||
                    GetIsDead(target) ||
                    GetDistanceBetween(self, target) > 40.0f)
                {
                    Enmity.RemoveFromEnmityTable(self, enmity.Value.Target);
                    continue;
                }

                AdjustReputation(target, self, -100);
            }
        }
    }
}
