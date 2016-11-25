using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs;
using CoffeBook.Model;
using BL;
using System.Collections.ObjectModel;

namespace CoffeBook.Helpers
{
    public class RecipeBookHelper
    {
        public static void RemoveRecipeBook(RecipeBook recipeBook)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            dbHandler.DeleteRecipeBookAsync(recipeBook.Id);
        }

        public static async Task<RecipeBook> AddRecipeBook(RecipeBook recipeBook)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();
            RecipeBookDto recipeBookDto = ConvertFromRecipeBook(recipeBook);
            return ConvertToRecipeBook(await dbHandler.AddRecipeBookAsync(recipeBookDto));
        }

        public static ICollection<RecipeBook> ConvertToRecipeBooks(IList<RecipeBookDto> recipeBookDtos)
        {
            ObservableCollection<RecipeBook> recipeBooks = new ObservableCollection<RecipeBook>();
            foreach (var recipeBookDto in recipeBookDtos)
            {
                recipeBooks.Add(ConvertToRecipeBook(recipeBookDto));
            }
            return recipeBooks;
        }

        public static IList<RecipeBookDto> ConvertFromRecipeBooks(ICollection<RecipeBook> recipeBooks)
        {
            List<RecipeBookDto> recipeBookDtos = new List<RecipeBookDto>();
            foreach (var recipeBook in recipeBooks)
            {
                recipeBookDtos.Add(ConvertFromRecipeBook(recipeBook));
            }
            return recipeBookDtos;
        }

        public static RecipeBook ConvertToRecipeBook(RecipeBookDto recipeBookDto)
        {
            RecipeBook recipeBook = new RecipeBook
            {
                Id = recipeBookDto.Id,
                Name = recipeBookDto.Name,
                Description = recipeBookDto.Description,
                Recipes = RecipeHelper.ConvertToRecipes(recipeBookDto.Recipes)
            };
            return recipeBook;
        }

        public static RecipeBookDto ConvertFromRecipeBook(RecipeBook recipeBook)
        {
            RecipeBookDto recipeBookDto = new RecipeBookDto
            {
                Id = recipeBook.Id,
                Name = recipeBook.Name,
                Description = recipeBook.Description,
                Recipes = RecipeHelper.ConvertFromRecipes(recipeBook.Recipes)
            };
            return recipeBookDto;
        }

        public static RecipeBook AddOrUpdateRecipeBook(RecipeBook recipeBook)
        {
            RecipeBook rb = null;
            if (recipeBook.Id <= 0)
            {
                //update
            } else
            {
                rb = AddRecipeBook(recipeBook).Result;
            }
            return rb;
        }
    }
}
