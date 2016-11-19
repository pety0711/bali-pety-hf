using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            using (var db = new CoffeBookContext())
            {
                try
                {
                    var coffees = db.Coffes.ToList();
                    var users = db.Users.ToList();
                    var recipes = db.Recipes.ToList();
                    var recipeBooks = db.RecipeBooks.ToList();
                    var userRecipeBooks = (from user in db.Users
                        where user.Id == 52
                        select user.RecipeBooks).ToList();
                    var rb = userRecipeBooks.ElementAt(0).ElementAt(0);
                    var bookrecipes = rb.Recipes;
                    var recipesInBook = (from recipeBook in db.RecipeBooks
                        where recipeBook.Id == 10
                        select recipeBook.Recipes).ToList();
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                    throw e;
                }
            }
        }


    }
}
