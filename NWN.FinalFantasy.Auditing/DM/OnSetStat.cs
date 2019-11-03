namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnSetStat : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Set Stat");
        }
    }
}