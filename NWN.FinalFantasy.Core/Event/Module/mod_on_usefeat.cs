using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Module;
using static NWN._;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class mod_on_usefeat
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnUseFeat, GetModule());
        }
    }
}