using NWN.FinalFantasy.Core;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class door_on_unlock
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, "ON_UNLOCK_");
        }
    }
}