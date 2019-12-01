using System;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.BlackMagic
{
    internal class AbilitySlot2 : IAbilityDefinition
    {
        public string Name => "Ability Slot II";
        public Feat Feat => Feat.AbilitySlot2;
        public AbilityCategory Category => AbilityCategory.Trait;
        public AbilityGroup Group => AbilityGroup.Individual;
        public EquipType EquipStatus => EquipType.SingleJob;
        public int APRequired => 0;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.Warrior, 15),
            new JobLevel(ClassType.Monk, 15),
            new JobLevel(ClassType.Thief, 15),
            new JobLevel(ClassType.Ranger, 15),
            new JobLevel(ClassType.WhiteMage, 15),
            new JobLevel(ClassType.BlackMage, 15),
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