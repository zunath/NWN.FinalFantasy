using System;

namespace NWN.FinalFantasy.Core
{
    internal class ApplicationSettings
    {
        public string DllSearchPattern { get; set; }
        public string DllDirectory { get; set; }
        public string NamespaceRoot { get; set; }

        public ApplicationSettings()
        {
            DllSearchPattern = Environment.GetEnvironmentVariable("FF_CORE_DLL_SEARCH_PATTERN");
            DllDirectory = Environment.GetEnvironmentVariable("FF_CORE_DLL_DIRECTORY");
            NamespaceRoot = Environment.GetEnvironmentVariable("FF_CORE_NAMESPACE_ROOT");
        }

    }
}
