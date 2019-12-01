using System;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition.BlackMagic
{
    internal class Fire: IAbilityDefinition
    {
        public string Name => "Fire";
        public Feat Feat => Feat.Fire;
        public AbilityCategory Category => AbilityCategory.Spell;
        public AbilityGroup Group => AbilityGroup.BlackMagic;
        public EquipType EquipStatus => EquipType.CrossJob;
        public int APRequired => 40;

        public JobLevel[] JobRequirements => new[]
        {
            new JobLevel(ClassType.BlackMage, 1), 
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
            return 1.5f;
        }

        public float CooldownTime(NWGameObject user)
        {
            return 1.0f;
        }

        public void Apply(NWGameObject user)
        {
        }

        public void Impact(NWGameObject user, NWGameObject target)
        {
            Console.WriteLine("impact from fire!");
        }
    }
}
