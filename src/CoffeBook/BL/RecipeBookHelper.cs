using DB;
using DB.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Model;

namespace BL
{
    public class RecipeBookHelper
    {
        public static ICollection<Model.RecipeBook> convertRecipeBooks(ICollection<DB.Entities.RecipeBook> recipeBooks)
        {
            if (recipeBooks == null)
            {
                return null;
            }
            ICollection<Model.RecipeBook> rbs = new ObservableCollection<Model.RecipeBook>();
            foreach (DB.Entities.RecipeBook recipeBook in recipeBooks)
            {
                BL.Model.RecipeBook rb = new Model.RecipeBook
                {
                    Name = recipeBook.Name,
                    Description = recipeBook.Description,
                    Id = recipeBook.Id,
                    Recipes = RecipeHelper.convertRecipes(recipeBook.Recipes)
                };
                rbs.Add(rb);
            }
            return rbs;
        }

        public static void GetRecipeBooks(long id)
        {
            
        }

        public static void RemoveRecipeBook(Model.RecipeBook recipeBook)
        {
            throw new NotImplementedException();
        }

        public static void AddRecipeBook(Model.RecipeBook recipeBook)
        {
            throw new NotImplementedException();
        }
    }
}
