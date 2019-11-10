namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnSetDate : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Set Date");
        }
    }
}