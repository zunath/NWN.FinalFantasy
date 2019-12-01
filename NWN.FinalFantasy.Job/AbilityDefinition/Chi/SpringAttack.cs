using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;
using static NWN._;

namespace NWN.FinalFantasy.Job.AbilityDefinition.StatBoost
{
    internal class SpringAttack : IAbilityDefinition
    {
        public string Name => "Spring Attack";
        public Feat Feat => Feat.SpringAttack;
        public AbilityCategory Category => AbilityCategory.Trait;
        public AbilityGroup Group => AbilityGroup.Individual;
        public EquipType EquipStatus => EquipType.CrossJob;
        public int APRequired => 50;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Monk, 8),
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