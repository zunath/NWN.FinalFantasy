using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event;
using NWN.FinalFantasy.Core.Messaging;

namespace NWN.FinalFantasy.Application
{
    internal class CustomEventRegistration
    {
        public static void Register()
        {
            MessageHub.Instance.Subscribe<CustomEvent>(OnCustomEvent);
        }

        private static void OnCustomEvent(CustomEvent payload)
        {
            var existingScriptData = Script.GetScriptData<object>(true);
            Script.SetScriptData(payload.Data);
            Script.RunScriptEvents(_.GetModule(), payload.ScriptPrefix);
            
            if(existingScriptData != null)
                Script.SetScriptData(existingScriptData);
            else 
                Script.ClearScriptData();
        }
    }
}
