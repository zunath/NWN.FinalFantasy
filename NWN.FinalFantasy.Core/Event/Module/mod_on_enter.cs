﻿using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Module;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class mod_on_enter
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerEnter);
        }
    }
}