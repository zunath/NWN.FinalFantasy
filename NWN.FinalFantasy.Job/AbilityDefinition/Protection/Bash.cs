using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.Protection
{
    internal class Bash: IAbilityDefinition
    {
        public string Name => "Bash";
        public Feat Feat => Feat.Bash;
        public AbilityCategory Category => AbilityCategory.Combat;
        public AbilityGroup Group => AbilityGroup.Individual;
        public EquipType EquipStatus => EquipType.CrossJob;
        public int APRequired => 80;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Warrior, 15),
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
