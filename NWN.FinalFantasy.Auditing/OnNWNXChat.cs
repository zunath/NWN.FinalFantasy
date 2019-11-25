using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing
{
    public class OnNWNXChat: IScript
    {
        public void Main()
        {
            var channel = NWNXChat.GetChannel();
            string log;

            // We don't log server messages because there isn't a good way to filter them.
            if (channel == NWNXChatChannel.ServerMessage) return;

            if (channel == NWNXChatChannel.DMTell ||
                channel == NWNXChatChannel.PlayerTell)
            {
                log = BuildTellLog();
            }
            else
            {
                log = BuildRegularLog();
            }

            Audit.Write(AuditGroup.Chat, log);
        }

        private static string BuildRegularLog()
        {
            var sender = NWNXChat.GetSender();
            var channel = NWNXChat.GetChannel();
            var message = NWNXChat.GetMessage();
            var ipAddress = GetPCIPAddress(sender);
            var cdKey = GetPCPublicCDKey(sender);
            var account = GetPCPlayerName(sender);
            var pcName = GetName(sender);

            var log = $"{pcName} - {account} - {cdKey} - {ipAddress} - {channel}: {message}";

            return log;
        }

        private static string BuildTellLog()
        {
            var sender = NWNXChat.GetSender();
            var receiver = NWNXChat.GetTarget();
            var channel = NWNXChat.GetChannel();
            var message = NWNXChat.GetMessage();
            var senderIPAddress = GetPCIPAddress(sender);
            var senderCDKey = GetPCPublicCDKey(sender);
            var senderAccount = GetPCPlayerName(sender);
            var senderPCName = GetName(sender);
            var receiverIPAddress = GetPCIPAddress(receiver);
            var receiverCDKey = GetPCPublicCDKey(receiver);
            var receiverAccount = GetPCPlayerName(receiver);
            var receiverPCName = GetName(receiver);

            var log = $"{senderPCName} - {senderAccount} - {senderCDKey} - {senderIPAddress} - {channel} (SENT TO {receiverPCName} - {receiverAccount} - {receiverCDKey} - {receiverIPAddress}): {message}";
            return log;
        }
    }
}
