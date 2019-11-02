using NWN.FinalFantasy.Core;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class store_on_close
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, "ON_CLOSED_");
        }
    }
}