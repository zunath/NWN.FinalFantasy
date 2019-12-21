using System.Collections.Generic;

namespace NWN.FinalFantasy.Crafting
{
    internal class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ItemResref { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public bool IsActive { get; set; }
        public List<Component> Components { get; set; }
    }
}
