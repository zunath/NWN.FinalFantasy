using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Crafting
{
    internal static class RecipeRegistry
    {
        private static readonly Dictionary<ClassType, Dictionary<int, Recipe>> _recipesByClass = new Dictionary<ClassType, Dictionary<int, Recipe>>();

        public static void Register()
        {
            _recipesByClass[ClassType.Blacksmith] = RegisterRecipeFile("BlacksmithRecipes.json");
            _recipesByClass[ClassType.Carpenter] = RegisterRecipeFile("CarpenterRecipes.json");
        }

        private static Dictionary<int, Recipe> RegisterRecipeFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = assembly.GetManifestResourceNames().Single(x => x.EndsWith(fileName));
            
            // Retrieve embedded json file and store it in a string.
            string json;
            using(Stream stream = assembly.GetManifestResourceStream(path))
            {
                if(stream == null)
                    throw new Exception("Unable to read the embedded JSON file '" + fileName + "'");

                using StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
            }

            // Deserialize the json data and add the recipes to the registry.
            var result = new Dictionary<int, Recipe>();
            var recipeList = JsonConvert.DeserializeObject<List<Recipe>>(json);

            foreach (var recipe in recipeList)
            {
                if(result.ContainsKey(recipe.ID))
                    throw new Exception($"Recipe ID '{recipe.ID}', '{recipe.Name}' has already been registered. IDs must be unique across each recipe list.");

                result[recipe.ID] = recipe;
            }

            return result;
        }

        /// <summary>
        /// Retrieves a recipe by a given class and ID.
        /// If either the class or ID have not been registered, an exception will be thrown.
        /// </summary>
        /// <param name="class">The class recipe list to use</param>
        /// <param name="id">The ID number of the recipe</param>
        /// <returns>A recipe matching a given class type and ID.</returns>
        public static Recipe GetByID(ClassType @class, int id)
        {
            if(!_recipesByClass.ContainsKey(@class))
                throw new Exception("Class '" + @class + "' has not been registered.");

            var recipes = _recipesByClass[@class];

            if(!recipes.ContainsKey(id))
                throw new Exception("Recipe with ID '" + id + "' has not been registered.");

            return recipes[id];
        }
    }
}
