using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Revives you, heals you to full, and restores all FP.", CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Rez: IChatCommand
    {
        /// <summary>
        /// Revives and heals user completely.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            if (GetIsDead(user))
            {
                ApplyEffectToObject(DurationType.Instant, EffectResurrection(), user);
            }

            ApplyEffectToObject(DurationType.Instant, EffectHeal(999), user);
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
