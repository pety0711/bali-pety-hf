using DB.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeBook.Converters
{
    public class UserConverter
    {

        private ICollection<CoffeBook.Model.RecipeBook> convertRecipeBook(ICollection<RecipeBook> recipeBooks)
        {
            ICollection<CoffeBook.Model.RecipeBook> rbs = new ObservableCollection<CoffeBook.Model.RecipeBook>();
            foreach (RecipeBook recipeBook in recipeBooks)
            {
                CoffeBook.Model.RecipeBook rb = new CoffeBook.Model.RecipeBook
                {
                    Name = recipeBook.Name,
                    Description = recipeBook.Description,
                    Id = recipeBook.Id,
                    Recipes = convertRecipes(recipeBook.Recipes)
                };
                rbs.Add(rb);
            }
            return rbs;
        }

        private ICollection<CoffeBook.Model.Recipe> convertRecipes(ICollection<Recipe> recipes)
        {
            ICollection<CoffeBook.Model.Recipe> rs = new ObservableCollection<CoffeBook.Model.Recipe>();
            foreach (Recipe r in recipes)
            {
                rs.Add(new CoffeBook.Model.Recipe
                {
                    Name = r.Name,
                    Picture = r.Picture,
                    Id = r.Id,
                    Description = r.Description,
                    CoffeType = coffeeConverter(r.CoffeeType)
                });
            }
            return rs;
        }

        private CoffeBook.Model.Coffee coffeeConverter(Coffee coffee)
        {
            CoffeBook.Model.Coffee c = new CoffeBook.Model.Coffee
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Picture = coffee.Picture
            };
            return c;
        }

    }
}
