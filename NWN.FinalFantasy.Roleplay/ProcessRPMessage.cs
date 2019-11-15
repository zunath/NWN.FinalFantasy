using System;
using System.Globalization;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Roleplay
{
    internal class ProcessRPMessage
    {
        private const string RPTimestampVariable = "RP_SYSTEM_LAST_MESSAGE_TIMESTAMP";

        public static void Main()
        {
            NWNXChatChannel channel = NWNXChat.GetChannel();
            NWGameObject player = NWNXChat.GetSender();
            if (!GetIsPlayer(player)) return;

            string message = NWNXChat.GetMessage().Trim();
            DateTime now = DateTime.UtcNow;

            bool isInCharacterChat =
                channel == NWNXChatChannel.PlayerTalk ||
                channel == NWNXChatChannel.PlayerWhisper ||
                channel == NWNXChatChannel.PlayerParty ||
                channel == NWNXChatChannel.PlayerShout;

            // Don't care about other chat channels.
            if (!isInCharacterChat) return;

            // Is the message too short?
            if (message.Length <= 3) return;

            // Is this an OOC message?
            var startingText = message.Substring(0, 2);
            if (startingText == "//" || startingText == "((") return;

            var playerID = GetGlobalID(player);
            var rpProgress = RoleplayProgressRepo.Get(playerID);

            // Spam prevention
            string timestampString = GetLocalString(player, RPTimestampVariable);
            SetLocalString(player, RPTimestampVariable, now.ToString(CultureInfo.InvariantCulture));

            // If there was a timestamp then we'll check for spam and prevent it from counting towards
            // the RP XP points.
            if (!string.IsNullOrWhiteSpace(timestampString))
            {
                DateTime lastSend = DateTime.Parse(timestampString);
                if (now <= lastSend.AddSeconds(1))
                {
                    rpProgress.SpamMessages++;
                    RoleplayProgressRepo.Set(playerID, rpProgress);
                    return;
                }
            }

            // Check if players are close enough for the channel in which the message was sent.
            if (!CanReceiveRPPoint(player, channel)) return;

            rpProgress.RPPoints++;
            RoleplayProgressRepo.Set(playerID, rpProgress);
        }

        private static bool CanReceiveRPPoint(NWGameObject player, NWNXChatChannel channel)
        {
            var playerID = GetGlobalID(player);

            // Party - Must be in a party with another PC.
            if (channel == NWNXChatChannel.PlayerParty)
            {
                var partyMember = GetFirstFactionMember(player);
                while (GetIsObjectValid(partyMember))
                {
                    if (GetGlobalID(partyMember) == playerID) continue;
                    return true;
                }

                return false;
            }

            var currentPlayer = GetFirstPC();
            while (GetIsObjectValid(currentPlayer))
            {
                float distance;
                if (channel == NWNXChatChannel.PlayerTalk)
                {
                    distance = 20.0f;
                }

                if (channel == NWNXChatChannel.PlayerWhisper)
                {
                    distance = 4.0f;
                }
                else break;

                if(GetDistanceBetween(player, currentPlayer) <= distance)
                {
                    return true;
                }

                currentPlayer = GetNextPC();
            }

            return false;
        }

    }
}
