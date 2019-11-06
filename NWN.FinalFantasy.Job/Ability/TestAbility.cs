using System;
using NWN.FinalFantasy.Job.Contracts;

namespace NWN.FinalFantasy.Job.Ability
{
    internal class TestAbility: IAbility
    {
        public int MP(NWGameObject user)
        {
            return 5;
        }

        public string CanUse(NWGameObject user, NWGameObject target)
        {
            return null;
        }

        public float CastingTime(NWGameObject user)
        {
            return 2f;
        }

        public float CooldownTime(NWGameObject user)
        {
            return 3f;
        }

        public void Impact(NWGameObject user, NWGameObject target)
        {
            Console.WriteLine("Running impact script");
        }

        public AbilityType Type => AbilityType.Spell;
    }
}
