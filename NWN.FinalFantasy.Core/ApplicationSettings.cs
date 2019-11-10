using System;

namespace NWN.FinalFantasy.Core
{
    public class ApplicationSettings
    {
        public string DllSearchPattern { get; }
        public string DllDirectory { get; }
        public string NamespaceRoot { get; }
        public string LogDirectory { get; }

        private static ApplicationSettings _settings;
        public static ApplicationSettings Get()
        {
            if(_settings == null)
                _settings = new ApplicationSettings();

            return _settings;
        }

        private ApplicationSettings()
        {
            DllSearchPattern = Environment.GetEnvironmentVariable("FF_CORE_DLL_SEARCH_PATTERN");
            DllDirectory = Environment.GetEnvironmentVariable("FF_CORE_DLL_DIRECTORY");
            NamespaceRoot = Environment.GetEnvironmentVariable("FF_CORE_NAMESPACE_ROOT");
            LogDirectory = Environment.GetEnvironmentVariable("FF_CORE_LOG_DIRECTORY");
        }

    }
}
