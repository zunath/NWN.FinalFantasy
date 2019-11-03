namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGetVariable : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Get Variable");
        }
    }
}