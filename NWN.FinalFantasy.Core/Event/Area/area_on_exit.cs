using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Area;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class area_on_exit
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaScriptPrefix.OnExit);
        }
    }
}