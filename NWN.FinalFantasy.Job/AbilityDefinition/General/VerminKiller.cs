using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.General
{
    internal class VerminKiller : IAbilityDefinition
    {
        public string Name => "Vermin Killer";
        public Feat Feat => Feat.FavoredEnemyVermin;
        public AbilityCategory Category => AbilityCategory.Trait;
        public AbilityGroup Group => AbilityGroup.Individual;
        public bool IsEquippable => true;
        public int APRequired => 40;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Warrior, 5),
            new JobLevel(ClassType.Monk, 5),
            new JobLevel(ClassType.Thief, 5),
            new JobLevel(ClassType.Ranger, 5),
            new JobLevel(ClassType.WhiteMage, 5),
            new JobLevel(ClassType.BlackMage, 5),
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