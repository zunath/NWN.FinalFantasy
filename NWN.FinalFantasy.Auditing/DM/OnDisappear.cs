namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnDisappear : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Disappear");
        }
    }
}