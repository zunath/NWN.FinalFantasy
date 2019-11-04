using System;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Report a bug to the developers. Please include as much detail as possible.", CommandPermissionType.Player | CommandPermissionType.DM | CommandPermissionType.Admin)]
    public class Bug : IChatCommand
    {
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            string message = string.Empty;

            foreach (var arg in args)
            {
                message += " " + arg;
            }

            if(message.Length > 1000)
            {
                SendMessageToPC(user, "Your message was too long. Please shorten it to no longer than 1000 characters and resubmit the bug. For reference, your message was: \"" + message + "\"");
                return;
            }

            var isPlayer = GetIsPlayer(user);
            var area = GetArea(user);
            var areaResref = GetResRef(area);
            var position = GetPosition(user);
            var orientation = GetFacing(user);

            BugReport report = new BugReport
            {
                SenderPlayerID = isPlayer ? new Guid?(GetGlobalID(user)): null,
                CDKey = GetPCPublicCDKey(user),
                Text = message,
                AreaResref = areaResref,
                SenderLocationX = position.X,
                SenderLocationY = position.Y,
                SenderLocationZ = position.X,
                SenderLocationOrientation = orientation
            };

            BugReportRepo.Set(report);
            SendMessageToPC(user, "Bug report submitted! Thank you for your report.");
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            if (args.Length <= 0 || args[0].Length <= 0)
            {
                return "Please enter in a description for the bug.";
            }

            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
