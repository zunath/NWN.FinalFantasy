using NWN.FinalFantasy.Core.Contracts;

namespace NWN.FinalFantasy.Core.Dialog
{
    public static class OnDialogStart
    {
        public static void Run()
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