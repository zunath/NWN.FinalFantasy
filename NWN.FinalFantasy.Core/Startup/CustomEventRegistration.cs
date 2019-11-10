using NWN.FinalFantasy.Core.Event;
using NWN.FinalFantasy.Core.Messaging;

namespace NWN.FinalFantasy.Core.Startup
{
    internal class CustomEventRegistration
    {
        public static void Register()
        {
            MessageHub.Instance.Subscribe<CustomEvent>(OnCustomEvent);
        }

        private static void OnCustomEvent(CustomEvent payload)
        {
            Script.SetScriptData(payload.Data);
            Script.RunScriptEvents(payload.Caller, payload.ScriptPrefix, _.GetModule());
            Script.ClearScriptData();
        }
    }
}
