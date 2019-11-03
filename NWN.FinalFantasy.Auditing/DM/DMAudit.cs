using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.NWNX;
using static NWN._;

namespace NWN.FinalFantasy.Auditing.DM
{
    public abstract class DMAudit
    {
        protected static void WriteLog(NWGameObject dm, string @event, string logDetails = null)
        {
            string ipAddress = GetPCIPAddress(dm);
            string cdKey = GetPCPublicCDKey(dm);
            string account = GetPCPlayerName(dm);
            string pcName = GetName(dm);

            string log = $"{pcName} - {account} - {cdKey} - {ipAddress} - {@event.ToUpper()}";

            if (!string.IsNullOrWhiteSpace(logDetails))
                log += ": " + logDetails;

            Audit.Write(AuditGroup.DM, log);
        }
    }
}
