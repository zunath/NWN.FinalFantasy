using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Area;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class area_on_user
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaPrefix.OnUserDefined);
        }
    }
}