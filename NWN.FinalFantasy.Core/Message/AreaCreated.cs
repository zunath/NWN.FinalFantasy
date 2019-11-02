namespace NWN.FinalFantasy.Core.Message
{
    internal class AreaCreated
    {
        public NWGameObject Area { get; set; }

        public AreaCreated(NWGameObject area)
        {
            Area = area;
        }
    }
}
