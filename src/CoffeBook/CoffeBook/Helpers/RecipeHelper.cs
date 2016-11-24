using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs;
using CoffeBook.Model;
using BL;

namespace CoffeBook.Helpers
{
    public class RecipeHelper
    {
        public static async ICollection<Recipe> GetRecipes()
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            var recipes = await dbHandler.GetAllRecipesAsync();
            return ConvertToRecipes(recipes);
        }

        public static void RemoveRecipe(Recipe recipe)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            dbHandler.DeleteRecipesAsync(recipe.Id);
        }

        public static async Task<Recipe> AddRecipe(Recipe recipe)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            var recipeDto = ConvertFromRecipe(recipe);
            return ConvertToRecipe(await dbHandler.AddRecipeAsync(recipeDto));
        }

        public static ICollection<Recipe> ConvertToRecipes(IList<RecipeDto> recipeDtos)
        {
            ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
            foreach (var recipeDto in recipeDtos)
            {
                recipes.Add(ConvertToRecipe(recipeDto));
            }
            return recipes;
        }

        public static IList<RecipeDto> ConvertFromRecipes(ICollection<Recipe> recipes)
        {
            List<RecipeDto> recipeDtos = new List<RecipeDto>();
            foreach (var recipe in recipes)
            {
                recipeDtos.Add(ConvertFromRecipe(recipe));
            }
            return recipeDtos;
        }

        public static Recipe ConvertToRecipe(RecipeDto recipeDto)
        {
            Recipe recipe = new Recipe
            {
                Id = recipeDto.Id,
                Name = recipeDto.Name,
                Description = recipeDto.Description,
                Picture = recipeDto.Picture,
                CoffeType = CoffeeHelper.ConvertToCoffee(recipeDto.CoffeeType)
            };
            return recipe;
        }

        public static RecipeDto ConvertFromRecipe(Recipe recipe)
        {
            RecipeDto recipeDto = new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Picture = recipe.Picture,
                CoffeeType = CoffeeHelper.ConvertFromCoffee(recipe.CoffeType)
            };
            return recipeDto;
        }

    }
}
