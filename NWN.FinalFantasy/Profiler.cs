using System;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy
{
    public class Metrics : IDisposable
    {
        public Metrics(string name)
        {
            Profiler.PushPerfScope(name);
        }

        public void Dispose()
        {
            Profiler.PopPerfScope();
        }
    }
}
