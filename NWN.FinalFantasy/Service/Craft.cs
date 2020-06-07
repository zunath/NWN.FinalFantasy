using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;
using NWN.FinalFantasy.Service.CraftService;

namespace NWN.FinalFantasy.Service
{
    public static class Craft
    {
        private static readonly Dictionary<RecipeType, RecipeDetail> _recipes = new Dictionary<RecipeType, RecipeDetail>();
        private static readonly Dictionary<RecipeCategoryType, RecipeCategoryAttribute> _allCategories = new Dictionary<RecipeCategoryType, RecipeCategoryAttribute>();
        private static readonly Dictionary<RecipeCategoryType, RecipeCategoryAttribute> _activeCategories = new Dictionary<RecipeCategoryType, RecipeCategoryAttribute>();
        private static readonly Dictionary<SkillType, Dictionary<RecipeCategoryType, Dictionary<RecipeType, RecipeDetail>>> _recipesBySkillAndCategory = new Dictionary<SkillType, Dictionary<RecipeCategoryType, Dictionary<RecipeType, RecipeDetail>>>();
        private static readonly Dictionary<SkillType, Dictionary<RecipeCategoryType, RecipeCategoryAttribute>> _categoriesBySkill = new Dictionary<SkillType, Dictionary<RecipeCategoryType, RecipeCategoryAttribute>>();

        /// <summary>
        /// When the skill cache has finished loading, recipe and category data is cached.
        /// </summary>
        [NWNEventHandler("ffo_skill_cached")]
        public static void CacheData()
        {
            CacheCategories();
            CacheRecipes();
        }

        /// <summary>
        /// When the module loads, all recipe categories are loaded into the cache.
        /// </summary>
        private static void CacheCategories()
        {
            var categories = Enum.GetValues(typeof(RecipeCategoryType)).Cast<RecipeCategoryType>();
            foreach (var category in categories)
            {
                var categoryDetail = category.GetAttribute<RecipeCategoryType, RecipeCategoryAttribute>();
                _allCategories[category] = categoryDetail;

                if (categoryDetail.IsActive)
                {
                    _activeCategories[category] = categoryDetail;
                }
            }
        }

        /// <summary>
        /// When the module loads, all recipes are loaded into the cache.
        /// </summary>
        private static void CacheRecipes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => typeof(IRecipeListDefinition).IsAssignableFrom(w) && !w.IsInterface && !w.IsAbstract);

            foreach (var type in types)
            {
                var instance = (IRecipeListDefinition)Activator.CreateInstance(type);
                var recipes = instance.BuildRecipes();

                foreach (var (recipeType, recipe) in recipes)
                {
                    _recipes[recipeType] = recipe;

                    // Organize recipe by skill and category.
                    if(!_recipesBySkillAndCategory.ContainsKey(recipe.Skill))
                        _recipesBySkillAndCategory[recipe.Skill] = new Dictionary<RecipeCategoryType, Dictionary<RecipeType, RecipeDetail>>();

                    if(!_recipesBySkillAndCategory[recipe.Skill].ContainsKey(recipe.Category))
                        _recipesBySkillAndCategory[recipe.Skill][recipe.Category] = new Dictionary<RecipeType, RecipeDetail>();

                    _recipesBySkillAndCategory[recipe.Skill][recipe.Category][recipeType] = recipe;

                    // Organize categories by skill based on whether there are any recipes under that category.
                    if (recipe.IsActive)
                    {
                        if (!_categoriesBySkill.ContainsKey(recipe.Skill))
                            _categoriesBySkill[recipe.Skill] = new Dictionary<RecipeCategoryType, RecipeCategoryAttribute>();

                        if (!_categoriesBySkill[recipe.Skill].ContainsKey(recipe.Category))
                            _categoriesBySkill[recipe.Skill][recipe.Category] = _allCategories[recipe.Category];

                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the details about a recipe.
        /// If recipe type has not been registered, an exception will be raised.
        /// </summary>
        /// <param name="recipeType">The type of recipe to retrieve.</param>
        /// <returns>The recipe detail.</returns>
        public static RecipeDetail GetRecipe(RecipeType recipeType)
        {
            return _recipes[recipeType];
        }

        /// <summary>
        /// Returns true if a recipe has been registered for this type.
        /// </summary>
        /// <param name="recipeType">The type of recipe to look for.</param>
        /// <returns>true if recipe has been registered, false otherwise.</returns>
        public static bool RecipeExists(RecipeType recipeType)
        {
            return _recipes.ContainsKey(recipeType);
        }

        /// <summary>
        /// Retrieves all of the recipes associated with a skill and category.
        /// </summary>
        /// <param name="skill">The skill to search by.</param>
        /// <param name="category">The category to search by.</param>
        /// <returns>A list of recipes under a given skill and category.</returns>
        public static Dictionary<RecipeType, RecipeDetail> GetRecipesBySkillAndCategory(SkillType skill, RecipeCategoryType category)
        {
            if(!_recipesBySkillAndCategory.ContainsKey(skill))
                return new Dictionary<RecipeType, RecipeDetail>();

            if(!_recipesBySkillAndCategory[skill].ContainsKey(category))
                return new Dictionary<RecipeType, RecipeDetail>();

            return _recipesBySkillAndCategory[skill][category].ToDictionary(x => x.Key, y => y.Value);
        }

        /// <summary>
        /// Retrieves all of the categories listed under a specific skill.
        /// </summary>
        /// <param name="skill">The skill to search by.</param>
        /// <returns>A list of recipe categories associated with a skill.</returns>
        public static Dictionary<RecipeCategoryType, RecipeCategoryAttribute> GetRecipeCategoriesBySkill(SkillType skill)
        {
            if(!_categoriesBySkill.ContainsKey(skill))
                return new Dictionary<RecipeCategoryType, RecipeCategoryAttribute>();

            return _categoriesBySkill[skill].ToDictionary(x => x.Key, y => y.Value);
        }

    }
}
