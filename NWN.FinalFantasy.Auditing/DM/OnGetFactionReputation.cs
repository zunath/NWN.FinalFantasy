namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnGetFactionReputation : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Get Faction Reputation");
        }
    }
}