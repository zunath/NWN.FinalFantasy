using System;
using Microsoft.Extensions.CommandLineUtils;

namespace NWN.FinalFantasy.CLI
{
    internal class Program
    {
        private static readonly FurnitureItemCreator _furnitureCreator = new FurnitureItemCreator();

        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            // Set up the options.
            var furnitureOption = app.Option(
                "-$|-f |--furniture",
                "Generates uti files in json format for all of the FurnitureType.cs enum values.",
                CommandOptionType.NoValue);

            app.HelpOption("-? | -h | --help");

            app.OnExecute(() =>
            {
                if (furnitureOption.HasValue())
                {
                    _furnitureCreator.Process();
                }

                return 0;
            });

            app.Execute(args);
        }
    }
}
