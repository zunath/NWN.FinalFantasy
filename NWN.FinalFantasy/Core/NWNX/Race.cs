using NWN.FinalFantasy.Core.NWNX.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Core.NWNX
{
    public class Race
    {
        private const string PLUGIN_NAME = "NWNX_Race";

        // Sets a racial modifier.
        public static void SetRacialModifier(
            RacialType race, 
            RaceModifiers modifier, 
            uint iParam1,
            uint iParam2 = 0xDEADBEEF, 
            uint iParam3 = 0xDEADBEEF)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetRacialModifier");
            Internal.NativeFunctions.nwnxPushInt((int)iParam3);
            Internal.NativeFunctions.nwnxPushInt((int)iParam2);
            Internal.NativeFunctions.nwnxPushInt((int)iParam1);
            Internal.NativeFunctions.nwnxPushInt((int)modifier);
            Internal.NativeFunctions.nwnxPushInt((int)race);
            Internal.NativeFunctions.nwnxCallFunction();
        }

        // Gets the parent race for a race.
        public static int GetParentRace(RacialType race)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetParentRace");
            Internal.NativeFunctions.nwnxPushInt((int)race);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }
    }
}