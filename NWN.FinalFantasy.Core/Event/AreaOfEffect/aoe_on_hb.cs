using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.AreaOfEffect;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class aoe_on_hb
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaOfEffectPrefix.OnHeartbeat);
        }
    }
}