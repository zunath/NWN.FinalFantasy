using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;
using static NWN._;

namespace NWN.FinalFantasy.Job.AbilityDefinition.StatBoost
{
    internal class HundredFists : IAbilityDefinition
    {
        public string Name => "Hundred Fists";
        public Feat Feat => Feat.HundredFists;
        public AbilityCategory Category => AbilityCategory.Combat;
        public AbilityGroup Group => AbilityGroup.Individual;
        public EquipType EquipStatus => EquipType.SingleJob;
        public int APRequired => 0;

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
            return 3f;
        }

        public float CooldownTime(NWGameObject user)
        {
            return 3600f;
        }

        public void Apply(NWGameObject user)
        {
        }

        public void Impact(NWGameObject user, NWGameObject target)
        {
            var effect = EffectLinkEffects(EffectHaste(), EffectModifyAttacks(3));
            ApplyEffectToObject(DurationType.Temporary, effect, user, 30.0f);
        }
    }
}