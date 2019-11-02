using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Core.Message
{
    internal class ObjectCreated
    {
        public ObjectType Type { get; set; }
        public NWGameObject GameObject { get; set; }

        public ObjectCreated(ObjectType type, NWGameObject gameObject)
        {
            Type = type;
            GameObject = gameObject;
        }
    }
}
