using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                    Console.WriteLine("Model compilation is currently disabled. Building hakpak without model compilation...");
                    CompileHakpak(hak.Name, hak.Path);

                    //var compilationFolder = $"{_config.OutputPath}compile_{hak.Name}/";
                    //Directory.CreateDirectory(compilationFolder);

                    //CompileModels(hak.Path, compilationFolder);
                    //CompileHakpak(hak.Name, compilationFolder);
                    //Directory.Delete(compilationFolder, true);
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

        /// <summary>
        /// Creates a new background process used for running external programs.
        /// </summary>
        /// <param name="command">The command to pass into the cmd instance.</param>
        /// <returns>A new process</returns>
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

        /// <summary>
        /// Compiles files contained in a folder into a hakpak.
        /// </summary>
        /// <param name="hakName">The name of the hak without the .hak extension</param>
        /// <param name="folderPath">The folder where the assets are.</param>
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

        /// <summary>
        /// Compiles all models found in a folder and places them into an output folder.
        /// Any files which don't have a .mdl extension will be copied to the output folder with no changes.
        /// CompileHakpak should be run on the output folder to build the hak.
        /// </summary>
        /// <param name="inputFolder">The source folder containing the assets to compile.</param>
        /// <param name="outputFolder">The output folder where the compiled (and non-compiled) assets will be moved to.</param>
        private void CompileModels(string inputFolder, string outputFolder)
        {
            // Start by compiling all mdl files
            var command = $"nwnmdlcomp {inputFolder}*.mdl {outputFolder} -e";

            Console.WriteLine($"Compiling models in folder: {inputFolder}");

            using (var process = CreateProcess(command))
            {
                process.Start();

                process.StandardInput.Flush();
                process.StandardInput.Close();

                process.StandardOutput.ReadToEnd();

                process.WaitForExit();
            }

            // Now move all non-mdl files over to the output folder.
            foreach (var file in Directory.GetFiles(inputFolder).Where(x => !x.EndsWith(".mdl")))
            {
                File.Copy(file, outputFolder + Path.GetFileName(file));
            }
        }
    }
}
