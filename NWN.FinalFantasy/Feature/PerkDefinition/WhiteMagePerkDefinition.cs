﻿using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class WhiteMagePerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            Benediction(builder);
            Cure(builder);
            Poisona(builder);
            Protectra(builder);
            Curaga(builder);
            Clarity(builder);
            Regen(builder);
            CombatMage(builder);
            TeleportBalamb(builder);
            Stone(builder);
            Dia(builder);

            return builder.Build();
        }

        private static void Benediction(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.WhiteMage, PerkType.Benediction)
                .Name("Benediction")
                .Description("You and all nearby party members are healed to full.")

                .AddPerkLevel()
                .Description("Grants the Benediction ability.")
                .RequirementSkill(SkillType.WhiteMagic, 50)
                .RequirementSkill(SkillType.Rod, 50)
                .RequirementSkill(SkillType.MysticArmor, 50)
                .Price(15);
        }

        private static void Cure(PerkBuilder builder)
        {

        }

        private static void Poisona(PerkBuilder builder)
        {

        }

        private static void Protectra(PerkBuilder builder)
        {

        }

        private static void Curaga(PerkBuilder builder)
        {

        }

        private static void Clarity(PerkBuilder builder)
        {

        }

        private static void Regen(PerkBuilder builder)
        {

        }

        private static void CombatMage(PerkBuilder builder)
        {

        }

        private static void TeleportBalamb(PerkBuilder builder)
        {

        }

        private static void Stone(PerkBuilder builder)
        {

        }

        private static void Dia(PerkBuilder builder)
        {

        }
    }
}