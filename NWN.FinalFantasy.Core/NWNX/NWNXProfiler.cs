namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXProfiler
    {
        public static void PushPerfScope(string name, string tag0_tag = "", string tag0_value = "")
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Profiler", "PUSH_PERF_SCOPE", name);

            if (tag0_value != "" && tag0_tag != "")
            {
                NWNXCore.NWNX_PushArgumentString("NWNX_Profiler", "PUSH_PERF_SCOPE", tag0_value);
                NWNXCore.NWNX_PushArgumentString("NWNX_Profiler", "PUSH_PERF_SCOPE", tag0_tag);
            }

            NWNXCore.NWNX_CallFunction("NWNX_Profiler", "PUSH_PERF_SCOPE");
        }

        public static void PopPerfScope()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Profiler", "POP_PERF_SCOPE");
        }
    }
}
