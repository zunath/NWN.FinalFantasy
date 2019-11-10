using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.NWScript.Enumerations;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class dialog_start
    {
        internal static void Main()
        {
            var pc = _.GetLastUsedBy();
            if (!_.GetIsObjectValid(pc)) pc = _.GetPCSpeaker();

            var talkTo = NWGameObject.OBJECT_SELF;
            string conversation = _.GetLocalString(talkTo, "CONVERSATION");

            if (!string.IsNullOrWhiteSpace(conversation))
            {
                Conversation.Start(pc, talkTo, conversation);
            }
            else
            {
                _.ActionStartConversation(pc, "", true, false);
            }
        }
    }
}