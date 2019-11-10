using NWN.FinalFantasy.Core.Event;

namespace NWN.FinalFantasy.Core.Messaging
{
    public static class Publish
    {
        public static void CustomEvent<T>(NWGameObject caller, string scriptPrefix, T data)
        {
            MessageHub.Instance.Publish(new CustomEvent(caller, scriptPrefix, data));
        }
    }
}
