using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Message;

namespace NWN.FinalFantasy.Job.Event
{
    internal class OnChangeJob
    {
        public static void Main()
        {
            Console.WriteLine("Changing job now");
            var data = Script.GetScriptData<JobChanged>();
            var newJob = data.NewJob;
            var player = NWGameObject.OBJECT_SELF;
            var playerID = _.GetGlobalID(player);
            var playerEntity = PlayerRepo.Get(playerID);
            var jobEntity = JobRepo.Get(playerID, newJob);
            playerEntity.CurrentJob = newJob;

            UnequipAllItems(player);
            AdjustNWNClass(player, newJob, jobEntity);
            
            PlayerRepo.Set(playerEntity);
        }

        /// <summary>
        /// Forces players to unequip all currently equipped items.
        /// </summary>
        /// <param name="player">The player unequipping all items</param>
        private static void UnequipAllItems(NWGameObject player)
        {
            _.AssignCommand(player, () => _.ClearAllActions());
            for (int x = 0; x < NWNConstants.NumberOfInventorySlots; x++)
            {
                NWGameObject item = _.GetItemInSlot((InventorySlot) x, player);

                if(_.GetIsObjectValid(item))
                {
                    _.AssignCommand(player, () => _.ActionUnequipItem(item));
                }
            }
        }

        private static void AdjustNWNClass(NWGameObject player, ClassType newJob, Data.Entity.Job jobEntity)
        {
            NWNXCreature.SetClassByPosition(player, ClassPosition.First, newJob);
            NWNXCreature.SetLevelByPosition(player, ClassPosition.First, jobEntity.Level);
        }
    }
}
