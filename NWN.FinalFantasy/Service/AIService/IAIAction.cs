using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAIAction
    {
        bool Action(List<uint> targets);
    }
}
