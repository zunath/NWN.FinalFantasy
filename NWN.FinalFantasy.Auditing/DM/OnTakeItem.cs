namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnTakeItem : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Take Item");
        }
    }
}