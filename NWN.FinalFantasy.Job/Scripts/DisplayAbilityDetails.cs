using System;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Utility;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    /// <summary>
    /// If an item has an ability attached, the details for that ability will be added to the description.
    /// Does not fire for any other types of objects.
    /// </summary>
    internal class DisplayAbilityDetails
    {
        public static void Main()
        {
            NWGameObject item = NWNXEvents.OnExamineObject_GetTarget();

            if (GetObjectType(item) != ObjectType.Item) return;

            var owner = GetItemPossessor(item);
            if (!GetIsObjectValid(owner) ||
                !GetIsPlayer(owner)) return;

            var abilityDetails = ColorToken.Orange("ABILITIES:\n\n");
            var playerID = GetGlobalID(owner);

            var ip = GetFirstItemProperty(item);
            while (GetIsItemPropertyValid(ip))
            {
                if (GetItemPropertyType(ip) == ItemPropertyType.Ability)
                {
                    // The IPRP_FFOAbility.2da file IDs should link up one-to-one with the Feat.2da file.
                    // Therefore, this conversion should work.
                    var feat = (Feat)GetItemPropertySubType(ip);
                    var progress = AbilityProgressRepo.Get(playerID, feat);
                    var abilityDefinition = AbilityRegistry.Get(feat);
                    string jobText = "( ";

                    foreach (var jobReq in abilityDefinition.JobRequirements)
                    {
                        var job = JobRegistry.Get(jobReq.Job);
                        jobText += $"{jobReq.Level}-{job.CallSign} ";
                    }

                    jobText += ")";

                    abilityDetails += $"{ColorToken.Orange(abilityDefinition.Name)} {jobText}\n    {ColorToken.Orange("AP:")} {progress.AP} / {abilityDefinition.APRequired}";

                    if (progress.AP >= abilityDefinition.APRequired)
                        abilityDetails += $" {ColorToken.Orange("[**MASTERED**]")}";
                    abilityDetails += "\n";
                }

                ip = GetNextItemProperty(item);
            }

            var description = GetDescription(item) + "\n\n" + abilityDetails;
            SetDescription(item, description);
        }
    }
}
