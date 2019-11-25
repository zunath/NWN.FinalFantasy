using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.Logging;
using static NWN._;

namespace NWN.FinalFantasy.Authorization
{
    public class OnModuleEnter: IScript
    {
        public void Main()
        {
            NWGameObject dm = GetEnteringObject();
            if (!GetIsDungeonMaster(dm)) return;

            var authorizationLevel = AuthorizationRegistry.GetAuthorizationLevel(dm);
            
            if (authorizationLevel != AuthorizationLevel.DM &&
                authorizationLevel != AuthorizationLevel.Admin)
            {
                LogDMAuthorization(false);
                BootPC(dm, "You are not authorized to log in as a DM. Please contact the server administrator if this is incorrect.");
                return;
            }

            LogDMAuthorization(true);
        }

        private static void LogDMAuthorization(bool success)
        {
            var player = GetEnteringObject();
            string ipAddress = GetPCIPAddress(player);
            string cdKey = GetPCPublicCDKey(player);
            string account = GetPCPlayerName(player);
            string pcName = GetName(player);

            if (success)
            {
                var log = $"{pcName} - {account} - {cdKey} - {ipAddress}: Authorization successful";
                Audit.Write(AuditGroup.DMAuthorization, log);
            }
            else
            {
                var log = $"{pcName} - {account} - {cdKey} - {ipAddress}: Authorization UNSUCCESSFUL";
                Audit.Write(AuditGroup.DMAuthorization, log);
            }

        }
    }
}
