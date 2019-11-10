using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Door;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class door_on_splcast
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnSpellCastAt);
        }
    }
}