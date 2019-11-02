namespace NWN.FinalFantasy.Core.Message
{
    internal class ObjectCreated
    {
        public NWGameObject GameObject { get; set; }

        public ObjectCreated(NWGameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
