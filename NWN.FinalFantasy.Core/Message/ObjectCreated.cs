namespace NWN.FinalFantasy.Core.Message
{
    public class ObjectCreated
    {
        public NWGameObject GameObject { get; set; }

        public ObjectCreated(NWGameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
