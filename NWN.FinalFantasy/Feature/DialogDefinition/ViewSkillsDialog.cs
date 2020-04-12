using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.DialogService;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class ViewSkillsDialog: DialogBase
    {
        private class Model
        {
            public SkillCategoryType SelectedCategoryID { get; set; }
            public SkillType SelectedSkillID { get; set; }
            public int RPXPDistributing { get; set; }
            public bool IsConfirming { get; set; }
        }

        public override PlayerDialog SetUp(uint player)
        {
            var dialog = DialogBuilder.Create()
                .WithDataModel(new Model())
                .AddPage("Please select a skill category.")
                .AddPage("Please select a skill.")
                .AddPage("<SET LATER>") // Distribute Roleplay XP
                .AddPage("<SET LATER>") // Roleplay XP Amount Distribution
                
                ;

            return null;
        }

        private void LoadCategoryPage()
        {

        }
    }
}
