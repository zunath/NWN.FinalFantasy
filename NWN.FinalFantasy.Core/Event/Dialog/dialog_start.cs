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

            string conversation = _.GetLocalString(NWGameObject.OBJECT_SELF, "CONVERSATION");

            if (!string.IsNullOrWhiteSpace(conversation))
            {
                var objectType = _.GetObjectType(NWGameObject.OBJECT_SELF);
                if (objectType == ObjectType.Placeable)
                {
                    var talkTo = NWGameObject.OBJECT_SELF;
                    Conversation.Start(pc, talkTo, conversation);
                }
                else
                {
                    var talkTo = NWGameObject.OBJECT_SELF;
                    Conversation.Start(pc, talkTo, conversation);
                }
            }
            else
            {
                _.ActionStartConversation(pc, "", true, false);
            }
        }
    }
}