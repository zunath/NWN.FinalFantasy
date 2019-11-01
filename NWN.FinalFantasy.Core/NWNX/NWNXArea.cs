namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXArea
    {

        const string NWNX_Area = "NWNX_Area";

        /// <summary>
        /// Gets the number of players in area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int GetNumberOfPlayersInArea(NWGameObject area)
        {
            string sFunc = "GetNumberOfPlayersInArea";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Gets the creature that last entered area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static NWGameObject GetLastEntered(NWGameObject area)
        {
            string sFunc = "GetLastEntered";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Gets the creature that last left area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static NWGameObject GetLastLeft(NWGameObject area)
        {
            string sFunc = "GetLastLeft";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the PVP setting of area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static AreaPVPSetting GetPVPSetting(NWGameObject area)
        {
            string sFunc = "GetPVPSetting";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return (AreaPVPSetting)NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the PVP setting of area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="pvpSetting"></param>
        public static void SetPVPSetting(NWGameObject area, AreaPVPSetting pvpSetting)
        {
            string sFunc = "SetPVPSetting";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, (int)pvpSetting);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the spot modifier of area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int GetAreaSpotModifier(NWGameObject area)
        {
            string sFunc = "GetAreaSpotModifier";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the spot modifier of area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="spotModifier"></param>
        public static void SetAreaSpotModifier(NWGameObject area, int spotModifier)
        {
            string sFunc = "SetAreaSpotModifier";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, spotModifier);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the listen modifer of area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int GetAreaListenModifier(NWGameObject area)
        {
            string sFunc = "GetAreaListenModifier";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the listen modifer of area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="listenModifier"></param>
        public static void SetAreaListenModifier(NWGameObject area, int listenModifier)
        {
            string sFunc = "SetAreaListenModifier";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, listenModifier);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Returns true if resting is not allowed in area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool GetNoRestingAllowed(NWGameObject area)
        {
            string sFunc = "GetNoRestingAllowed";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc) == 1;
        }

        /// <summary>
        /// Set whether resting is allowed in area
        /// true: Resting not allowed
        /// false: Resting allowed
        /// </summary>
        /// <param name="area"></param>
        /// <param name="bNoRestingAllowed"></param>
        public static void SetNoRestingAllowed(NWGameObject area, bool bNoRestingAllowed)
        {
            string sFunc = "SetNoRestingAllowed";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, bNoRestingAllowed ? 1 : 0);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the wind power in area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int GetWindPower(NWGameObject area)
        {
            string sFunc = "GetWindPower";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the wind power in area
        /// windPower = 0-2
        /// </summary>
        /// <param name="area"></param>
        /// <param name="windPower"></param>
        public static void SetWindPower(NWGameObject area, int windPower)
        {
            string sFunc = "SetWindPower";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, windPower);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the weather chance of type in area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetWeatherChance(NWGameObject area, AreaWeatherChance type)
        {
            string sFunc = "GetWeatherChance";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, (int)type);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the weather chance of type in area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="type"></param>
        /// <param name="chance"></param>
        public static void SetWeatherChance(NWGameObject area, AreaWeatherChance type, int chance)
        {
            string sFunc = "SetWeatherChance";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, chance);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, (int)type);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the fog clip distance in area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static float GetFogClipDistance(NWGameObject area)
        {
            string sFunc = "GetFogClipDistance";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueFloat(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the fog clip distance in area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="distance"></param>
        public static void SetFogClipDistance(NWGameObject area, float distance)
        {
            string sFunc = "SetFogClipDistance";

            NWNXCore.NWNX_PushArgumentFloat(NWNX_Area, sFunc, distance);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the shadow opacity of area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int GetShadowOpacity(NWGameObject area)
        {
            string sFunc = "GetShadowOpacity";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the shadow opacity of area
        /// shadowOpacity = 0-100
        /// </summary>
        /// <param name="area"></param>
        /// <param name="shadowOpacity"></param>
        public static void SetShadowOpacity(NWGameObject area, int shadowOpacity)
        {
            string sFunc = "SetShadowOpacity";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, shadowOpacity);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Get the day/night cycle of area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static AreaDayNightCycle GetDayNightCycle(NWGameObject area)
        {
            string sFunc = "GetDayNightCycle";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return (AreaDayNightCycle)NWNXCore.NWNX_GetReturnValueInt(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the day/night cycle of area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="type"></param>
        public static void SetDayNightCycle(NWGameObject area, AreaDayNightCycle type)
        {
            string sFunc = "SetDayNightCycle";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, (int)type);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Set the Sun/Moon Ambient/Diffuse colors of area
        /// type = NWNX_AREA_COLOR_TYPE_*
        /// color = FOG_COLOR_*
        ///
        /// The color can also be represented as a hex RGB number if specific color shades are desired.
        /// The format of a hex specified color would be 0xFFEEDD where
        /// FF would represent the amount of red in the color
        /// EE would represent the amount of green in the color
        /// DD would represent the amount of blue in the color.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        public static void SetSunMoonColors(NWGameObject area, AreaColorType type, int color)
        {
            string sFunc = "SetSunMoonColors";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, color);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Area, sFunc, (int)type);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);
        }

        /// <summary>
        /// Create and returns a transition (square shaped of specified size) at a location
        /// Valid object types for the target are DOOR or WAYPOINT.
        /// If a tag is specified the returning object will have that tag
        /// </summary>
        /// <param name="area"></param>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="size"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static NWGameObject CreateTransition(NWGameObject area, NWGameObject target, float x, float y, float z, float size = 2.0f, string tag = "")
        {
            string sFunc = "CreateTransition";

            NWNXCore.NWNX_PushArgumentString(NWNX_Area, sFunc, tag);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Area, sFunc, size);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Area, sFunc, z);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Area, sFunc, y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Area, sFunc, x);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, target);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Area, sFunc, area);
            NWNXCore.NWNX_CallFunction(NWNX_Area, sFunc);

            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Area, sFunc);
        }
    }
}
