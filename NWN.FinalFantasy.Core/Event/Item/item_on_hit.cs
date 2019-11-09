using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Area;
using NWN.FinalFantasy.Core.Event.Item;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class item_on_hit
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ItemEventPrefix.OnItemHitCastSpell, _.GetModule());
        }
    }
}