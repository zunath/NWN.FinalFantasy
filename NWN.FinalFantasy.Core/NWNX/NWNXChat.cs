namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXChat
    {
        public const int NWNX_CHAT_CHANNEL_PLAYER_TALK = 1;
        public const int NWNX_CHAT_CHANNEL_PLAYER_SHOUT = 2;
        public const int NWNX_CHAT_CHANNEL_PLAYER_WHISPER = 3;
        public const int NWNX_CHAT_CHANNEL_PLAYER_TELL = 4;
        public const int NWNX_CHAT_CHANNEL_SERVER_MSG = 5;
        public const int NWNX_CHAT_CHANNEL_PLAYER_PARTY = 6;
        public const int NWNX_CHAT_CHANNEL_PLAYER_DM = 14;
        public const int NWNX_CHAT_CHANNEL_DM_TALK = 17;
        public const int NWNX_CHAT_CHANNEL_DM_SHOUT = 18;
        public const int NWNX_CHAT_CHANNEL_DM_WHISPER = 19;
        public const int NWNX_CHAT_CHANNEL_DM_TELL = 20;
        public const int NWNX_CHAT_CHANNEL_DM_PARTY = 22;
        public const int NWNX_CHAT_CHANNEL_DM_DM = 30;

        // Sends a chat message. Channel is a NWNX_* constant.
        // If no target is provided, then it broadcasts to all eligible targets.
        // Returns TRUE if successful, FALSE otherwise.
        public static int SendMessage(int channel, string message, NWGameObject sender, NWGameObject target)
        {
            NWNXCore.NWNX_PushArgumentObject("NWNX_Chat", "SEND_MESSAGE", target);
            NWNXCore.NWNX_PushArgumentObject("NWNX_Chat", "SEND_MESSAGE", sender);
            NWNXCore.NWNX_PushArgumentString("NWNX_Chat", "SEND_MESSAGE", message);
            NWNXCore.NWNX_PushArgumentInt("NWNX_Chat", "SEND_MESSAGE", channel);
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "SEND_MESSAGE");
            return NWNXCore.NWNX_GetReturnValueInt("NWNX_Chat", "SEND_MESSAGE");
        }

        // Registers the script which receives all chat messages.
        // If a script was previously registered, this one will take over.
        public static void RegisterChatScript(string script)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Chat", "REGISTER_CHAT_SCRIPT", script);
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "REGISTER_CHAT_SCRIPT");
        }

        // Skips the message.
        // Must be called from an chat or system script handler.
        public static void SkipMessage()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "SKIP_MESSAGE");
        }

        // Gets the channel. Channel is a NWNX_* constant.
        // Must be called from an chat or system script handler.
        public static int GetChannel()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "GET_CHANNEL");
            return NWNXCore.NWNX_GetReturnValueInt("NWNX_Chat", "GET_CHANNEL");
        }

        // Gets the message.
        // Must be called from an chat or system script handler.
        public static string GetMessage()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "GET_MESSAGE");
            return NWNXCore.NWNX_GetReturnValueString("NWNX_Chat", "GET_MESSAGE");
        }

        // Gets the sender.
        // Must be called from an chat or system script handler.
        public static NWGameObject GetSender()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "GET_SENDER");
            return (NWNXCore.NWNX_GetReturnValueObject("NWNX_Chat", "GET_SENDER"));
        }

        // Gets the target. May be OBJECT_INVALID if no target.
        // Must be called from an chat or system script handler.
        public static NWGameObject GetTarget()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Chat", "GET_TARGET");
            return (NWNXCore.NWNX_GetReturnValueObject("NWNX_Chat", "GET_TARGET"));
        }

    }
}
