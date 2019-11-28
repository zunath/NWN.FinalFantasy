namespace NWN.FinalFantasy.Core.Event
{
    public class CustomEvent
    {
        public NWGameObject Caller { get; set; }
        public string ScriptPrefix { get; set; }
        public object Data { get; set; }

        public CustomEvent(NWGameObject caller, string scriptPrefix, object data)
        {
            Caller = caller;
            ScriptPrefix = scriptPrefix;
            Data = data;
        }
    }
}
