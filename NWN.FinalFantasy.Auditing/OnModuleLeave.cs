using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using static NWN._;

namespace NWN.FinalFantasy.Auditing
{
    public class OnModuleLeave: IScript
    {
        public void Main()
        {
            var player = GetExitingObject();
            string ipAddress = GetPCIPAddress(player);
            string cdKey = GetPCPublicCDKey(player);
            string account = GetPCPlayerName(player);
            string pcName = GetName(player);

            var log = $"{pcName} - {account} - {cdKey} - {ipAddress}: Left the server";
            Audit.Write(AuditGroup.Connection, log);
        }
    }
}
