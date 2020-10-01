using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Service.AIService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Type = NWN.FinalFantasy.Core.NWScript.Enum.Creature.Type;

namespace NWN.FinalFantasy.Feature.AIDefinition.AITargetDefinition
{
    public class NearestTarget: IAITargets
    {
        public async Task<List<uint>> GetTargetsAsync(uint creature)
        {
            var nearest = OBJECT_INVALID;
            var task = NWTask.Run(() =>
            {
                //Console.WriteLine($"Requesting nearest creature on NWN thread. ID = {Thread.CurrentThread.ManagedThreadId}. Self = {GetName(creature)}");
                nearest = GetNearestCreature(Type.PlayerCharacter, 1, creature);
                return Task.CompletedTask;
            });

            await NWTask.WhenAll(task);

            return new List<uint>{ nearest };
        }
    }
}
