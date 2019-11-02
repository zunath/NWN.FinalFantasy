using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Encounter;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class enc_on_exit
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnExit);
        }
    }
}