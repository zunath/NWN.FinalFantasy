using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using static NWN._;

namespace NWN.FinalFantasy.Auditing
{
    public class OnModuleEnter: IScript
    {
        public void Main()
        {
            var player = GetEnteringObject();
            string ipAddress = GetPCIPAddress(player);
            string cdKey = GetPCPublicCDKey(player);
            string account = GetPCPlayerName(player);
            string pcName = GetName(player);

            var log = $"{pcName} - {account} - {cdKey} - {ipAddress}: Connected to server";
            Audit.Write(AuditGroup.Connection, log);
        }
    }
}
