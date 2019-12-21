using System;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public class LearnedRecipeRepo
    {
        private static string BuildKey(Guid playerID, ClassType @class, int recipeID)
        {
            return $"LearnedRecipe:{playerID}:{@class}:{recipeID}";
        }

        public static LearnedRecipe Get(Guid playerID, ClassType @class, int recipeID)
        {
            var key = BuildKey(playerID, @class, recipeID);

            if (!DB.Exists(key))
            {
                var entity = new LearnedRecipe
                {
                    Class = @class,
                    RecipeID = recipeID
                };

                Set(playerID, entity);
                return entity;
            }

            return DB.Get<LearnedRecipe>(key);
        }

        public static void Set(Guid playerID, LearnedRecipe entity)
        {
            var key = BuildKey(playerID, entity.Class, entity.RecipeID);
            DB.Set(key, entity);
        }
    }
}