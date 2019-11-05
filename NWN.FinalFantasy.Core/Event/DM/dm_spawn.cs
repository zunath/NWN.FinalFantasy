using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.DM;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWNX;


// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class dm_spawn
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        public static void Main()
        {
            var obj = NWNXEvents.OnDMSpawnObject_GetObject();
            MessageHub.Instance.Publish(new ObjectCreated(obj));

            ScriptRunner.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSpawnObject);
        }
    }
}