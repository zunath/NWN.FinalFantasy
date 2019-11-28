using System.Linq;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.AI.Scripts.NPCEnmity
{
    public class AttackHighestEnmity: IScript
    {
        public void Main()
        {
            var self = NWGameObject.OBJECT_SELF;
            var enmityTable = Enmity.GetOrCreateEnmityTable(self);
            var target = enmityTable.Values
                .OrderByDescending(o => o.Amount)
                .FirstOrDefault(x => GetIsObjectValid(x.Target) && GetArea(x.Target) == GetArea(self));

            var currentAttackTarget = GetAttackTarget(self);

            // We have a target and it's not who we're currently attacking. Switch to attacking them.
            if (target != null && currentAttackTarget != target.Target)
            {
                AssignCommand(self, () =>
                {
                    ClearAllActions();
                    ActionAttack(target.Target);
                });
            }
            // We don't have a valid target but we're still attacking someone. We shouldn't be attacking them anymore. Clear all actions.
            else if (target == null && GetCurrentAction(self) == ActionType.AttackObject)
            {
                AssignCommand(self, () =>
                {
                    ClearAllActions();
                });
            }
        }
    }
}
