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
    public class UserHelper
    {
        public static Model.User GetUser(string name)
        {
            using (var context = new CoffeBookContext())
            {
                DB.Entities.User user = context.Users.Where(u => u.Name == name).SingleOrDefault();
                return user == null ? null : convertUser(user);
            }
        }

        private static Model.User convertUser(DB.Entities.User user)
        {
            Model.User u = new Model.User
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                RecipeBooks = convertRecipeBooks(user.RecipeBooks)
            };
            return u;
        }

        private static ICollection<Model.RecipeBook> convertRecipeBooks(ICollection<DB.Entities.RecipeBook> recipeBooks)
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
                    Recipes = convertRecipes(recipeBook.Recipes)
                };
                rbs.Add(rb);
            }
            return rbs;
        }

        private static ICollection<Model.Recipe> convertRecipes(ICollection<DB.Entities.Recipe> recipes)
        {
            if (recipes == null)
            {
                return null;
            }
            ICollection<Model.Recipe> rs = new ObservableCollection<Model.Recipe>();
            foreach (DB.Entities.Recipe r in recipes) {
                rs.Add(new BL.Model.Recipe
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

        private static Model.Coffee coffeeConverter(DB.Entities.Coffee coffee)
        {
            if (coffee == null)
            {
                return null;
            }
            Model.Coffee c = new Model.Coffee
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Picture = coffee.Picture
            };
            return c;
        }

        public static void RegisterUser(Model.User loginUser)
        {
            using (var context = new CoffeBookContext())
            {
                DB.Entities.User newUser = new DB.Entities.User
                {
                    Name = loginUser.Name,
                    Password = loginUser.Password
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }
    }
}
