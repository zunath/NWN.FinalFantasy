namespace NWN.FinalFantasy.Core.Message
{
    public class AreaCreated
    {
        public NWGameObject Area { get; set; }

        public AreaCreated(NWGameObject area)
        {
            Area = area;
        }
    }
}
