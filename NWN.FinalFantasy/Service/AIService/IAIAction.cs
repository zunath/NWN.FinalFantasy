using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAIAction
    {
        void Action(uint creature, params uint[] targets);
    }
}
