using BL.Model;
using DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RecipeHelper
    {
        public static ICollection<Recipe> GetRecipes()
        {
            using (var context = new CoffeBookContext())
            {
                return convertRecipes(context.Recipes.ToList());
            }
        }

        public static ICollection<Recipe> GetRecipes(string name)
        {
            using (var context = new CoffeBookContext())
            {
                ICollection<DB.Entities.Recipe> recipes = context.Recipes.Where(r => r.Name.ToLower() == name.ToLower()).ToList();
                return convertRecipes(recipes);
            }
        }

        public static Recipe GetRecipe(string name)
        {
            using (var context = new CoffeBookContext())
            {
                ICollection<DB.Entities.Recipe> recipes = context.Recipes.Where(r => r.Name.ToLower() == name.ToLower()).ToList();
                return convertRecipes(recipes).FirstOrDefault();
            }
        }

        public static Recipe GetRecipe(int id)
        {
            using (var context = new CoffeBookContext())
            {
                ICollection<DB.Entities.Recipe> recipes = context.Recipes.Where(r => r.Id == id).ToList();
                return convertRecipes(recipes).FirstOrDefault();
            }
        }

        public static ICollection<Model.Recipe> convertRecipes(ICollection<DB.Entities.Recipe> recipes)
        {
            if (recipes == null)
            {
                return null;
            }
            ICollection<Model.Recipe> rs = new ObservableCollection<Model.Recipe>();
            foreach (DB.Entities.Recipe r in recipes)
            {
                rs.Add(new BL.Model.Recipe
                {
                    Name = r.Name,
                    Picture = r.Picture,
                    Id = r.Id,
                    Description = r.Description,
                    CoffeType = CoffeeHelper.coffeeConverter(r.CoffeeType)
                });
            }
            return rs;
        }

    }
}
