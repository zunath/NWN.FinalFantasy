namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnAppear: DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Appear");
        }
    }
}
