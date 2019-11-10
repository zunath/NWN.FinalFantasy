using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Job.Scripts
{
    internal class UnequipAllItems
    {
        public static void Main()
        {
            var player = NWGameObject.OBJECT_SELF;

            _.AssignCommand(player, () => _.ClearAllActions());
            for (int x = 0; x < NWNConstants.NumberOfInventorySlots; x++)
            {
                NWGameObject item = _.GetItemInSlot((InventorySlot)x, player);

                if (_.GetIsObjectValid(item))
                {
                    _.AssignCommand(player, () => _.ActionUnequipItem(item));
                }
            }
        }
    }
}
