using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Inventory;
using static NWN._;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class inv_rem_item
    {
        internal static void Main()
        {
            if (!GetIsObjectValid(NWGameObject.OBJECT_SELF))
            {
                return;
            }

            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, InventoryPrefix.OnRemoveItem);
        }
    }
}