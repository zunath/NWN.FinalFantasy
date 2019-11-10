using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Placeable;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class plc_on_damage
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnDamaged);
        }
    }
}