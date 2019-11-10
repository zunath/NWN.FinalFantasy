using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Placeable;
using NWN.FinalFantasy.Core.Event.Server;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class server_on_discon
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ServerEventPrefix.OnDisconnect, _.GetModule());
        }
    }
}