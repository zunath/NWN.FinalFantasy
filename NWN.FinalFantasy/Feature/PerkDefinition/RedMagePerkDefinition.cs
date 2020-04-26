using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class RedMagePerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            Manafont(builder);
            Protect(builder);
            TransferMP(builder);
            TransferStamina(builder);
            PiercingStab(builder);
            Blind(builder);
            RecoveryStab(builder);
            Convert(builder);
            Refresh(builder);
            RapierFinesse(builder);
            Jolt(builder);
            PoisonStab(builder);
            ShockSpikes(builder);
            DeliberateStab(builder);

            return builder.Build();
        }

        private static void Manafont(PerkBuilder builder)
        {

        }

        private static void Protect(PerkBuilder builder)
        {

        }

        private static void TransferMP(PerkBuilder builder)
        {

        }

        private static void TransferStamina(PerkBuilder builder)
        {

        }

        private static void PiercingStab(PerkBuilder builder)
        {

        }

        private static void Blind(PerkBuilder builder)
        {

        }

        private static void RecoveryStab(PerkBuilder builder)
        {

        }

        private static void Convert(PerkBuilder builder)
        {

        }

        private static void Refresh(PerkBuilder builder)
        {

        }

        private static void RapierFinesse(PerkBuilder builder)
        {

        }

        private static void Jolt(PerkBuilder builder)
        {

        }

        private static void PoisonStab(PerkBuilder builder)
        {

        }

        private static void ShockSpikes(PerkBuilder builder)
        {

        }

        private static void DeliberateStab(PerkBuilder builder)
        {

        }
    }
}
