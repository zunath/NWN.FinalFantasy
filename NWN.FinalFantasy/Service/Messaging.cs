using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Type = NWN.FinalFantasy.Core.NWScript.Enum.Creature.Type;

namespace NWN.FinalFantasy.Service
{
    public class Messaging
    {
        /// <summary>
        /// Sends a message to all nearby players within 10 meters.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The message to send to all nearby players.</param>
        public static void SendMessageNearbyToPlayers(uint sender, string message)
        {
            const float MaxDistance = 10.0f;

            SendMessageToPC(sender, message);

            int nth = 1;
            var nearby = GetNearestCreature(Type.PlayerCharacter, 1, sender, nth);
            while (GetIsObjectValid(nearby) && GetDistanceBetween(sender, nearby) <= MaxDistance)
            {
                if (sender == nearby) continue;

                SendMessageToPC(nearby, message);
                nth++;
                nearby = GetNearestCreature(Type.PlayerCharacter, 1, sender, nth);
            }
        }
    }
}
