using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.Protection
{
    internal class SpikedDefense: IAbilityDefinition
    {
        public string Name => "Spiked Defense";
        public Feat Feat => Feat.SpikedDefense;
        public AbilityCategory Category => AbilityCategory.Combat;
        public AbilityGroup Group => AbilityGroup.Individual;
        public EquipType EquipStatus => EquipType.SingleJob;
        public int APRequired => 70;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Warrior, 8), 
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
            var effect = _.EffectDamageShield(1, DamageBonus.OneD4, DamageType.Sonic);
            var vfx = _.EffectVisualEffect(Vfx.Vfx_Imp_Death_Ward);

            _.ApplyEffectToObject(DurationType.Instant, vfx, target);
            _.ApplyEffectToObject(DurationType.Temporary, effect, target, 60.0f);
        }
    }
}
