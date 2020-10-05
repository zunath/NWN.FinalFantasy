using System;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Core
{
    public class Profiler : IDisposable
    {
        public Profiler(string name)
        {
            NWNXProfiler.PushPerfScope(name, "RunScript", "Script");
        }

        public void Dispose()
        {
            NWNXProfiler.PopPerfScope();
        }
    }
}
