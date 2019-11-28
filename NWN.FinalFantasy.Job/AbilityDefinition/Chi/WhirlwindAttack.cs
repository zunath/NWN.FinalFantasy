using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;
using static NWN._;

namespace NWN.FinalFantasy.Job.AbilityDefinition.StatBoost
{
    internal class WhirlwindAttack : IAbilityDefinition
    {
        public string Name => "Whirlwind Attack";
        public Feat Feat => Feat.WhirlwindAttack;
        public AbilityCategory Category => AbilityCategory.Combat;
        public AbilityGroup Group => AbilityGroup.Individual;
        public bool IsEquippable => false;
        public int APRequired => 90;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Monk, 13),
        };
        public int MP(NWGameObject user)
        {
            return 0;
        }

        public string CanUse(NWGameObject user, NWGameObject target)
        {
            return null;
        }

        public float CastingTime(NWGameObject user)
        {
            return 0f;
        }

        public float CooldownTime(NWGameObject user)
        {
            return 0f;
        }

        public void Apply(NWGameObject user)
        {
        }

        public void Impact(NWGameObject user, NWGameObject target)
        {
        }
    }
}