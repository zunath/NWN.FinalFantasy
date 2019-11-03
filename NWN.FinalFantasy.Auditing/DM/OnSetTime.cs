namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnSetTime : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Set Time");
        }
    }
}