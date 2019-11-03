namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnDumpLocalVariables : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Dump Local Variables");
        }
    }
}