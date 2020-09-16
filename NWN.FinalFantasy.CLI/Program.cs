using System;
using Microsoft.Extensions.CommandLineUtils;

namespace NWN.FinalFantasy.CLI
{
    internal class Program
    {
        private static readonly FurnitureItemCreator _furnitureCreator = new FurnitureItemCreator();
        private static readonly HakBuilder _hakBuilder = new HakBuilder();

        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            // Set up the options.
            var furnitureOption = app.Option(
                "-$|-f |--furniture",
                "Generates uti files in json format for all of the FurnitureType.cs enum values.",
                CommandOptionType.NoValue);

            var hakBuilderOption = app.Option(
                "-$|-k |--hak",
                "Builds hakpak files based on the hakbuilder.json configuration file.",
                CommandOptionType.NoValue);

            app.HelpOption("-? | -h | --help");

            app.OnExecute(() =>
            {
                if (furnitureOption.HasValue())
                {
                    _furnitureCreator.Process();
                }

                if (hakBuilderOption.HasValue())
                {
                    _hakBuilder.Process();
                }

                return 0;
            });

            app.Execute(args);
        }
    }
}
