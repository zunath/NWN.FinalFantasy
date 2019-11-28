using NWN.FinalFantasy.Core.Contracts;
using Serilog;
using Serilog.Events;

namespace NWN.FinalFantasy.Chat
{
    public class ChatRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Write(LogEventLevel.Information, "Registering Chat...");
            ChatCommandRegistry.Initialize();
        }
    }
}
