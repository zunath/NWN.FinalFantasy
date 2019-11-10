using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Item
{
    /// <summary>
    /// Resets an item's description back to its original text.
    /// You should call this as the first step in the examine item pipeline.
    /// </summary>
    internal class ResetDescription
    {
        public static void Main()
        {
            NWGameObject item = NWNXEvents.OnExamineObject_GetTarget();
            if (GetObjectType(item) != ObjectType.Item) return;

            string backupDescription = GetLocalString(item, "DESCRIPTION_BACKUP");

            if (string.IsNullOrWhiteSpace(backupDescription))
            {
                backupDescription = GetDescription(item);
                SetLocalString(item, "DESCRIPTION_BACKUP", backupDescription);
                return;
            }

            SetDescription(item, backupDescription);
        }
    }
}
