using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.StatBoost
{
    internal class HPBoost1: IAbilityDefinition
    {
        public string Name => "HP Boost";
        public Feat Feat => Feat.HPBoost1;
        public AbilityCategory Category => AbilityCategory.Trait;
        public AbilityGroup Group => AbilityGroup.Individual;
        public EquipType EquipStatus => EquipType.CrossJob;
        public int APRequired => 70;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Warrior, 7),
            new JobLevel(ClassType.Monk, 2),
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
            return 0;
        }

        public float CooldownTime(NWGameObject user)
        {
            return 0;
        }

        public void Apply(NWGameObject user)
        {
        }

        public void Impact(NWGameObject user, NWGameObject target)
        {
        }
    }
}
