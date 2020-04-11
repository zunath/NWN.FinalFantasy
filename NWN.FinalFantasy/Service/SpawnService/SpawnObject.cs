using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Service.SpawnService
{
    public class SpawnObject
    {
        public ObjectType Type { get; set; }
        public string Resref { get; set; }
        public int Weight { get; set; }
    }
}
