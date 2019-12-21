using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class LearnedRecipe: EntityBase
    {
        public int RecipeID { get; set; }
        public ClassType Class { get; set; }
    }
}
