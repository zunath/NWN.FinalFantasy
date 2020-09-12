using NWN.FinalFantasy.Core.NWNX.Enum;

namespace NWN.FinalFantasy.Core.NWNX
{
    public static class Util
    {
        private const string PLUGIN_NAME = "NWNX_Util";

        public static string GetCurrentScriptName(int depth = 0)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetCurrentScriptName");
            Internal.NativeFunctions.nwnxPushInt(depth);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopString();
        }

        public static string GetAsciiTableString()
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetAsciiTableString");
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopString();
        }

        public static int Hash(string str)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "Hash");
            Internal.NativeFunctions.nwnxPushString(str);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static string GetCustomToken(int customTokenNumber)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "customTokenNumber");
            Internal.NativeFunctions.nwnxPushInt(customTokenNumber);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopString();
        }

        public static Core.ItemProperty EffectToItemProperty(Core.Effect effect)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "EffectTypeCast");
            Internal.NativeFunctions.nwnxPushEffect(effect.Handle);
            Internal.NativeFunctions.nwnxCallFunction();
            return new Core.ItemProperty(Internal.NativeFunctions.nwnxPopItemProperty());
        }

        public static Core.Effect ItemPropertyToEffect(Core.ItemProperty ip)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "EffectTypeCast");
            Internal.NativeFunctions.nwnxPushItemProperty(ip.Handle);
            Internal.NativeFunctions.nwnxCallFunction();
            return new Core.Effect(Internal.NativeFunctions.nwnxPopEffect());
        }

        public static int IsValidResRef(string resRef, int type = (int)ResRefType.Creature)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "IsValidResRef");
            Internal.NativeFunctions.nwnxPushInt(type);
            Internal.NativeFunctions.nwnxPushString(resRef);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static int GetMinutesPerHour()
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetMinutesPerHour");
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static void SetMinutesPerHour(int minutes)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetMinutesPerHour");
            Internal.NativeFunctions.nwnxPushInt(minutes);
            Internal.NativeFunctions.nwnxCallFunction();
        }

        public static string EncodeStringForURL(string url)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "EncodeStringForURL");
            Internal.NativeFunctions.nwnxPushString(url);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopString();
        }

        public static int Get2DARowCount(string str)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "Get2DARowCount");
            Internal.NativeFunctions.nwnxPushString(str);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static string GetFirstResRef(ResRefType type, string regexFilter = "", bool moduleResourcesOnly = true)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetFirstResRef");
            Internal.NativeFunctions.nwnxPushInt(moduleResourcesOnly ? 1 : 0);
            Internal.NativeFunctions.nwnxPushString(regexFilter);
            Internal.NativeFunctions.nwnxPushInt((int)type);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopString();
        }

        public static string GetNextResRef()
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetNextResRef");
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopString();
        }

        public static int GetServerTicksPerSecond()
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetServerTicksPerSecond");
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        /// @brief Get the world time as calendar day and time of day.
        /// @note This function is useful for calculating effect expiry times.
        /// @param fAdjustment An adjustment in seconds, 0.0f will return the current world time,
        /// positive or negative values will return a world time in the future or past.
        /// @return A NWNX_Util_WorldTime struct with the calendar day and time of day.
        public static WorldTime GetWorldTime(float fAdjustment = 0.0f)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetWorldTime");

            Internal.NativeFunctions.nwnxPushFloat(fAdjustment);
            Internal.NativeFunctions.nwnxCallFunction();

            return new WorldTime
            {
                TimeOfDay = Internal.NativeFunctions.nwnxPopInt(),
                CalendarDay = Internal.NativeFunctions.nwnxPopInt()
            };
        }

        public struct WorldTime
        {
            public int CalendarDay { get; set; }
            public int TimeOfDay { get; set; }
        }


        public static void SetResourceOverride(int nResType, string sOldName, string sNewName)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetResourceOverride");

            Internal.NativeFunctions.nwnxPushString(sNewName);
            Internal.NativeFunctions.nwnxPushString(sOldName);
            Internal.NativeFunctions.nwnxPushInt(nResType);

            Internal.NativeFunctions.nwnxCallFunction();
        }

        public static string GetResourceOverride(int nResType, string sName)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetResourceOverride");

            Internal.NativeFunctions.nwnxPushString(sName);
            Internal.NativeFunctions.nwnxPushInt(nResType);
            Internal.NativeFunctions.nwnxCallFunction();

            return Internal.NativeFunctions.nwnxPopString();
        }
    }
}