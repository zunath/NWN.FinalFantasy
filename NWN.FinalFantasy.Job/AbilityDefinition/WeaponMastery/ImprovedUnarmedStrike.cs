using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.WeaponMastery
{
    internal class ImprovedUnarmedStrike: IAbilityDefinition
    {
        public string Name => "Improved Unarmed Strike";
        public Feat Feat => Feat.ImprovedUnarmedStrike;
        public AbilityCategory Category => AbilityCategory.Trait;
        public AbilityGroup Group => AbilityGroup.Individual;
        public bool IsEquippable => false;
        public int APRequired => 50;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Monk, 1), 
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
