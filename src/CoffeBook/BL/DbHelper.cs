using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
using DB.Entities;

namespace BL
{
    public class DbHelper
    {
        public static void TestDb()
        {
            using (var context = new CoffeBookContext())
            {
                var conn = context.Database.Connection.ConnectionString;
                context.Database.Delete();
                context.Database.CreateIfNotExists();
                var user = new User
                {
                    Name = "Feri",
                    Password = "teve"
                };
                var coffee = new Coffee { Name = "Kofi" };
                var recipe = new Recipe
                {
                    Name = "Kofi Recipe",
                    CoffeeType = coffee,
                    Description = "Put in stuff you like and drink."
                };
                var recipe1 = new Recipe
                {
                    Name = "Kofi Recipe1",
                    CoffeeType = coffee,
                    Description = "Put in stuff you like and drink."
                };
                var recipe2 = new Recipe
                {
                    Name = "Kofi Recipe2",
                    CoffeeType = coffee,
                    Description = "Put in stuff you like and drink."
                };
                var recipe3 = new Recipe
                {
                    Name = "Kofi Recipe3",
                    CoffeeType = coffee,
                    Description = "Put in stuff you like and drink."
                };
                var recipeBook = new RecipeBook
                {
                    Name = "Good Recipes",
                    Recipes = new List<Recipe> { recipe, recipe1, recipe2 }
                };
                user.RecipeBooks = new List<RecipeBook> { recipeBook };
                context.Recipes.Add(recipe);
                context.Recipes.Add(recipe1);
                context.Recipes.Add(recipe2);
                context.Recipes.Add(recipe3);
                context.RecipeBooks.Add(recipeBook);
                context.Users.Add(user);
                context.Coffes.Add(coffee);
                context.SaveChanges();
            }
            using (var db = new CoffeBookContext())
            {
                var coffees = db.Coffes.ToList();
                var users = db.Users.ToList();
                var recipes = db.Recipes.ToList();
                var recipeBooks = db.RecipeBooks.ToList();
                var userRecipeBooks = (from user in db.Users
                                       where user.Name.Equals("Feri")
                                       select user.RecipeBooks).ToList();
                var recipesInBook = (from recipeBook in db.RecipeBooks
                                     where recipeBook.Name.Equals("Good Recipes")
                                     select recipeBook.Recipes).ToList();
            }
        }


    }
}
