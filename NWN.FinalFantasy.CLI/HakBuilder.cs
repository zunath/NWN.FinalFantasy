using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NWN.FinalFantasy.CLI.Model;

namespace NWN.FinalFantasy.CLI
{
    public class HakBuilder
    {
        private const string ConfigFilePath = "./hakbuilder.json";
        private HakBuilderConfig _config;

        public void Process()
        {
            // Read the config file.
            _config = GetConfig();

            // Clean the output folder.
            CleanOutputFolder();

            // Copy the TLK to the output folder.
            Console.WriteLine($"Copying TLK: {_config.TlkPath}");
            File.Copy(_config.TlkPath, $"{_config.OutputPath}{Path.GetFileName(_config.TlkPath)}");

            // Iterate over every configured hakpak folder and compile it.
            Parallel.ForEach(_config.HakList, hak =>
            {
                if (hak.CompileModels)
                {
                    // todo: compile step
                    CompileHakpak(hak.Name, hak.Path);
                }
                else
                {
                    CompileHakpak(hak.Name, hak.Path);
                }
            });

        }

        /// <summary>
        /// Retrieves the configuration file for the hak builder.
        /// Throws an exception if the file is missing.
        /// </summary>
        /// <returns>The hak builder config settings.</returns>
        private HakBuilderConfig GetConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                throw new Exception($"Unable to locate config file. Ensure file '{ConfigFilePath}' exists in the same folder as this application.");
            }

            var json = File.ReadAllText(ConfigFilePath);

            return JsonConvert.DeserializeObject<HakBuilderConfig>(json);
        }

        /// <summary>
        /// Cleans the output folder.
        /// </summary>
        private void CleanOutputFolder()
        {
            if (Directory.Exists(_config.OutputPath))
            {
                Directory.Delete(_config.OutputPath, true);
            }

            Directory.CreateDirectory(_config.OutputPath);
        }

        private Process CreateProcess(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd.exe", "/K " + command)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = false
                },
                EnableRaisingEvents = true
            };

            return process;
        }

        private void CompileHakpak(string hakName, string folderPath)
        {
            var command = $"nwn_erf -f \"{_config.OutputPath}{hakName}.hak\" -e HAK -c ./{folderPath}";
            Console.WriteLine($"Building hak: {hakName}.hak");

            using (var process = CreateProcess(command))
            {
                process.Start();

                process.StandardInput.Flush();
                process.StandardInput.Close();

                process.StandardOutput.ReadToEnd();

                process.WaitForExit();
            }
        }

    }
}
