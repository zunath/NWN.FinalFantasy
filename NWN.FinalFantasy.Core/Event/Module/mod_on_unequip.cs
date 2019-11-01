using NWN.FinalFantasy.Core.Event;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class mod_on_unequip
    {
        public static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, "ON_UNEQUIP_ITEM_");
        }
    }
}