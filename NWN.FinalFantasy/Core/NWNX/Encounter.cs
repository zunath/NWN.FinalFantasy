using NWN.FinalFantasy.Core.NWNX.Enum;


namespace NWN.FinalFantasy.Core.NWNX
{
    public static class Encounter
    {
        private const string PLUGIN_NAME = "NWNX_Encounter";

        public static int GetNumberOfCreaturesInEncounterList(uint encounter)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetNumberOfCreaturesInEncounterList");
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static CreatureListEntry GetEncounterCreatureByIndex(uint encounter, int index)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetEncounterCreatureByIndex");
            Internal.NativeFunctions.nwnxPushInt(index);
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();

            return new CreatureListEntry
            {
                unique = Internal.NativeFunctions.nwnxPopInt(),
                challengeRating = Internal.NativeFunctions.nwnxPopFloat(),
                resref = Internal.NativeFunctions.nwnxPopString()
            };
        }

        public static void SetEncounterCreatureByIndex(uint encounter, int index, CreatureListEntry creatureEntry)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetEncounterCreatureByIndex");
            Internal.NativeFunctions.nwnxPushInt(creatureEntry.unique);
            Internal.NativeFunctions.nwnxPushFloat(creatureEntry.challengeRating);
            Internal.NativeFunctions.nwnxPushString(creatureEntry.resref!);
            Internal.NativeFunctions.nwnxPushInt(index);
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
        }

        public static int GetFactionId(uint encounter)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetFactionId");
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static void SetFactionId(uint encounter, int factionId)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetFactionId");
            Internal.NativeFunctions.nwnxPushInt(factionId);
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
        }

        public static int GetPlayerTriggeredOnly(uint encounter)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetPlayerTriggeredOnly");
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static void SetPlayerTriggeredOnly(uint encounter, int playerTriggeredOnly)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetPlayerTriggeredOnly");
            Internal.NativeFunctions.nwnxPushInt(playerTriggeredOnly);
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
        }

        public static int GetResetTime(uint encounter)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetResetTime");
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
            return Internal.NativeFunctions.nwnxPopInt();
        }

        public static void SetResetTime(uint encounter, int resetTime)
        {
            Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetResetTime");
            Internal.NativeFunctions.nwnxPushInt(resetTime);
            Internal.NativeFunctions.nwnxPushObject(encounter);
            Internal.NativeFunctions.nwnxCallFunction();
        }
    }
}