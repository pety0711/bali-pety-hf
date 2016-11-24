using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Entities;

namespace DB
{
    class CoffeeBookInitializer : CreateDatabaseIfNotExists<CoffeBookContext>
    {
        protected override void Seed(CoffeBookContext context)
        {
            for (int i = 1; i <= 100; i++)
            {
                var coffee = new Coffee { Name = "Coffee " + i };
                context.Coffes.Add(coffee);
                var recipe = new Recipe
                {
                    Name = "Recipe " + i,
                    CoffeeType = coffee,
                    Description = "Recipe of coffee " + i
                };
                context.Recipes.Add(recipe);
                var user = new User
                {
                    Name = "User " + i,
                    Password = "Batman",
                    FbPassword = "SocialBatman",
                    FbMail = "me@bat.cave",
                    RecipeBooks = new List<RecipeBook>()
                };
                for (int j = 1; j <= 3; j++)
                {
                    var recipeBook = new RecipeBook();
                    recipeBook.Name = $"Recipebook {i}_{j}";
                    recipeBook.Description = $"Recipebook recipes {i}_{j}";
                    recipeBook.Recipes = new List<Recipe> {recipe};
                    user.RecipeBooks.Add(recipeBook);
                    context.RecipeBooks.Add(recipeBook);
                }
                context.Users.Add(user);
            }
            base.Seed(context);
        }
    }
}
