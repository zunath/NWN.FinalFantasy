using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace NWN.FinalFantasy.Core
{
    internal class ApplicationSettings
    {
        public string DllSearchPattern { get; set; }
        public string DllDirectory { get; set; }
        public string NamespaceRoot { get; set; }

        private static ApplicationSettings _instance;
        public static ApplicationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadSettings();
                }

                return _instance;
            }
        }

        private static ApplicationSettings LoadSettings()
        {
            var @namespace = Assembly.GetExecutingAssembly().GetName().Name;
            const string FileName = "AppSettings.json";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@namespace + "." + FileName))
            {
                if (stream == null)
                {
                    throw new Exception("Unable to locate AppSettings.json");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<ApplicationSettings>(json);
                }
            }
        }
    }
}
