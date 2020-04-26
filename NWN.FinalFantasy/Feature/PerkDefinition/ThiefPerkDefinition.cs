using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class ThiefPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            PerfectDodge(builder);
            Opportunist(builder);
            DaggerFinesse(builder);
            Steal(builder);
            Gilfinder(builder);
            TreasureHunter(builder);
            WaspSting(builder);
            Shadowstich(builder);
            LifeSteal(builder);
            SneakAttack(builder);
            Hide(builder);
            Flee(builder);

            return builder.Build();
        }

        private static void PerfectDodge(PerkBuilder builder)
        {

        }

        private static void Opportunist(PerkBuilder builder)
        {

        }

        private static void DaggerFinesse(PerkBuilder builder)
        {

        }

        private static void Steal(PerkBuilder builder)
        {

        }

        private static void Gilfinder(PerkBuilder builder)
        {

        }

        private static void TreasureHunter(PerkBuilder builder)
        {

        }

        private static void WaspSting(PerkBuilder builder)
        {

        }

        private static void Shadowstich(PerkBuilder builder)
        {

        }

        private static void LifeSteal(PerkBuilder builder)
        {

        }

        private static void SneakAttack(PerkBuilder builder)
        {

        }

        private static void Hide(PerkBuilder builder)
        {

        }

        private static void Flee(PerkBuilder builder)
        {

        }
    }
}
