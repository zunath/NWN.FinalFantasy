using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;
using static NWN._;

namespace NWN.FinalFantasy.Job.AbilityDefinition.Protection
{
    internal class HallowedGround: IAbilityDefinition
    {
        public string Name => "Hallowed Ground";
        public Feat Feat => Feat.HallowedGround;
        public AbilityCategory Category => AbilityCategory.Combat;
        public AbilityGroup Group => AbilityGroup.Individual;
        public bool IsEquippable => false;
        public int APRequired => 0;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Warrior, 1), 
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
            SetPlotFlag(user, true);
            ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(Vfx.Vfx_Imp_Ac_Bonus), user);
            DelayCommand(30.0f, () =>
            {
                SetPlotFlag(user, false);
                SendMessageToPC(user, "Your Hallowed Ground effect has worn off.");
            });
        }
    }
}
