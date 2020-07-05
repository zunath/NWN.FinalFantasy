using System;
using System.IO;
using System.Linq;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;

namespace NWN.FinalFantasy.CLI
{
    internal class FurnitureItemCreator
    {
        public void Process()
        {
            const string Output = "./furniture_output/";
            const string Template = "furniture_0000.uti.json";

            if (Directory.Exists(Output))
            {
                Directory.Delete(Output, true);
            }

            Directory.CreateDirectory(Output);

            var templateContents = File.ReadAllText($"./Templates/{Template}");


            var furnitureTypes = Enum.GetValues(typeof(FurnitureType)).Cast<FurnitureType>();
            foreach (var furniture in furnitureTypes)
            {
                var furnitureDetail = furniture.GetAttribute<FurnitureType, FurnitureAttribute>();

                var id = ((int) furniture).ToString().PadLeft(4, '0');
                var fileName = Template.Replace("0000", id);

                var contents = templateContents
                    .Replace("%%NAME%%", furnitureDetail.Name)
                    .Replace("%%TAG%%", $"furniture_{id}")
                    .Replace("%%RESREF%%", $"furniture_{id}");

                File.WriteAllText($"{Output}{fileName}", contents);
            }
        }
    }
}
