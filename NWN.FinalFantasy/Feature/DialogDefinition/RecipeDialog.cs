using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class RecipeDialog: DialogBase
    {
        private class Model
        {
            public SkillType SelectedSkill { get; set; }
            public RecipeCategoryType SelectedCategory { get; set; }
            public RecipeType SelectedRecipe { get; set; }
            public bool IsFabricator { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string CategoryPageId = "CATEGORY_PAGE";
        private const string RecipeListPageId = "RECIPE_LIST_PAGE";
        private const string RecipePageId = "RECIPE_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var model = new Model();

            // This menu can be entered one of two ways:
            //    1.) From the player's rest menu.
            //    2.) From using a crafting fabricator.
            // If the second event happens, we need to skip over the first page.
            var skillType = (SkillType)GetLocalInt(player, "SKILL_TYPE_ID");
            model.SelectedSkill = skillType;
            DeleteLocalInt(player, "SKILL_TYPE_ID");

            if (model.SelectedSkill != SkillType.Invalid)
            {
                model.IsFabricator = true;
            }

            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddBackAction(Back)
                .AddPage(MainPageId, MainPageInit)
                .AddPage(CategoryPageId, CategoryPageInit)
                .AddPage(RecipeListPageId, RecipeListPageInit)
                .AddPage(RecipePageId, RecipePageInit);

            return builder.Build();
        }

        private void Back(string oldPage, string newPage)
        {
            if (newPage == MainPageId)
            {
                var model = GetDataModel<Model>();
                model.SelectedCategory = RecipeCategoryType.Invalid;
                model.SelectedRecipe = RecipeType.Invalid;

                if (!model.IsFabricator)
                    model.SelectedSkill = SkillType.Invalid;
            }
        }

        private void MainPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();

            if (model.IsFabricator)
            {
                ChangePage(CategoryPageId);
                return;
            }

            page.Header = "Please select a skill.";

            page.AddResponse(Skill.GetSkillDetails(SkillType.Blacksmithing).Name, () =>
            {
                model.SelectedSkill = SkillType.Blacksmithing;
                ChangePage(CategoryPageId);
            });

            page.AddResponse(Skill.GetSkillDetails(SkillType.Leathercraft).Name, () =>
            {
                model.SelectedSkill = SkillType.Leathercraft;
                ChangePage(CategoryPageId);
            });

            page.AddResponse(Skill.GetSkillDetails(SkillType.Alchemy).Name, () =>
            {
                model.SelectedSkill = SkillType.Alchemy;
                ChangePage(CategoryPageId);
            });

            page.AddResponse(Skill.GetSkillDetails(SkillType.Cooking).Name, () =>
            {
                model.SelectedSkill = SkillType.Cooking;
                ChangePage(CategoryPageId);
            });
        }


        private void CategoryPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            page.Header = "Please select a category.";

            foreach (var (key, value) in Craft.GetRecipeCategoriesBySkill(model.SelectedSkill))
            {
                page.AddResponse(value.Name, () =>
                {
                    model.SelectedCategory = key;
                });
            }
        }

        private void RecipeListPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            page.Header = "Please select a recipe.";

            foreach (var (key, value) in Craft.GetRecipesBySkillAndCategory(model.SelectedSkill, model.SelectedCategory))
            {
                page.AddResponse(value.Name, () =>
                {
                    model.SelectedRecipe = key;
                });
            }
        }

        private void RecipePageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();

            string BuildHeader()
            {
                return string.Empty;
            }

            page.Header = BuildHeader();

        }

    }
}
