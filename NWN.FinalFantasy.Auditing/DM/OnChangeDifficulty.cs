using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Auditing.DM
{
    public class OnChangeDifficulty : DMAudit, IScript
    {
        public void Main()
        {
            var dm = NWGameObject.OBJECT_SELF;
            var difficulty = NWNXEvents.OnDMChangeDifficulty_GetDifficultySetting();
            WriteLog(dm, "Change Difficulty", "New Setting: " + difficulty);
        }
    }
}