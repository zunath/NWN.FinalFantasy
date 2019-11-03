namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnSetFactionReputation : DMAudit
    {
        public static void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            WriteLog(dm, "Set Faction Reputation");
        }
    }
}