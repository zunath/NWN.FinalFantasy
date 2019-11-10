using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Store;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class store_on_open
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, StorePrefix.OnOpen);
        }
    }
}