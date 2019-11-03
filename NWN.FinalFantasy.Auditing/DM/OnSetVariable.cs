namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnSetVariable : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Set Variable");
        }
    }
}