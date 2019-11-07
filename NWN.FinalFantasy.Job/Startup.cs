using NWN.FinalFantasy.Job.Registry;

namespace NWN.FinalFantasy.Job
{
    public static class Startup
    {
        public static void Main()
        {
            AbilityRegistry.Register();
            JobRegistry.Register();
            RatingRegistry.Register();
        }
    }
}
