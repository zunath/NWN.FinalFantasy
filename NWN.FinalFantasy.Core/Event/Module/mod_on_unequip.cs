using NWN.FinalFantasy.Core;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class mod_on_unequip
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, "ON_UNEQUIP_ITEM_");
        }
    }
}