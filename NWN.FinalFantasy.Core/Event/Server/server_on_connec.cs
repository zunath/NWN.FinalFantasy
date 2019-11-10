using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Placeable;
using NWN.FinalFantasy.Core.Event.Server;
using static NWN._;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class server_on_connec
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ServerEventPrefix.OnConnect, GetModule());
        }
    }
}