using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Entities;

namespace DB
{
    public class CoffeBookContext : DbContext
    {
        public DbSet<Coffee> Coffes { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RecipeBook> RecipeBooks { get; set; }

        private static bool s_recreateDatabase = true; 

        public CoffeBookContext()
        {
            if (s_recreateDatabase)
            {
                Database.Delete();
                s_recreateDatabase = false;
            }
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new CoffeeBookInitializer());
        }
    }
}
