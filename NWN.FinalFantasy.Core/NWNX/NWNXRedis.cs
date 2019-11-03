using System;
using static NWN.FinalFantasy.Core.NWNX.NWNXCore;

namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXRedis
    {
        public struct PubSubMessageData
        {
            public string channel;
            public string message;
        };

        public static PubSubMessageData GetPubSubMessageData()
        {
            PubSubMessageData ret;
            NWNX_CallFunction("NWNX_Redis", "GetPubSubData");
            ret.message = NWNX_GetReturnValueString("NWNX_Redis", "GetPubSubData");
            ret.channel = NWNX_GetReturnValueString("NWNX_Redis", "GetPubSubData");
            return ret;
        }

        public static void Set(string key, string value, string condition = "")
        {
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", "SET");
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", key);
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", value);
            if (condition != "") NWNX_PushArgumentString("NWNX_Redis", "Deferred", condition);
            NWNX_CallFunction("NWNX_Redis", "Deferred");
        }

        public static string Get(string sKey)
        {
            return ResultToString(GetResult(sKey));
        }

        private static string ResultToString(int resultId)
        {
            NWNX_PushArgumentInt("NWNX_Redis", "GetResultAsString", resultId);
            NWNX_CallFunction("NWNX_Redis", "GetResultAsString");
            return NWNX_GetReturnValueString("NWNX_Redis", "GetResultAsString");
        }

        private static int GetResult(string key)
        {
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", "GET");
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", key);
            NWNX_CallFunction("NWNX_Redis", "Deferred");
            return NWNX_GetReturnValueInt("NWNX_Redis", "Deferred");
        }

        public static int PUBSUB(string subcommand, string argument = "")
        {
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", "PUBSUB");
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", subcommand);
            if (argument != "") NWNX_PushArgumentString("NWNX_Redis", "Deferred", argument);
            NWNX_CallFunction("NWNX_Redis", "Deferred");
            return NWNX_GetReturnValueInt("NWNX_Redis", "Deferred");
        }

        public static bool Exists(string key)
        {
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", "EXISTS");
            NWNX_PushArgumentString("NWNX_Redis", "Deferred", key);
            NWNX_CallFunction("NWNX_Redis", "Deferred");
            return Convert.ToBoolean(NWNX_GetReturnValueInt("NWNX_Redis", "Deferred"));
        }

    }
}
