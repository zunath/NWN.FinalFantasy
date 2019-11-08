using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Encounter;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class enc_on_enter
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnEnter);
        }
    }
}