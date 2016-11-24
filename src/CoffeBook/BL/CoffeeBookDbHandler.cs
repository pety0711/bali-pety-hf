using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using BL.DTOs;
using BL.Interfaces;
using DB;
using DB.Entities;

namespace BL
{

    public class CoffeeBookDbHandler : IDbHandler
    {

        public async Task<BlCallResult<RecipeDto>> AddRecipeToCoffee(CoffeeDto coffee, RecipeDto recipe)
        {
            Console.WriteLine("AddRecipeToCoffee enter");
            using (var db = new CoffeBookContext())
            {
                var coffeeFromDb = db.Coffes.SingleOrDefault(x => x.Id == coffee.Id);
                if (coffeeFromDb == null)
                {
                    try
                    {
                        await AddCoffeeAsync(coffee);
                        coffeeFromDb = db.Coffes.SingleOrDefault(x => x.Id == coffee.Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("AddRecipeToCoffee exception");
                        return new BlCallResult<RecipeDto>(BlCallResult.BlResult.CoffeeError, e);
                    }
                }
                var recipeFromDb = GetRecipe(recipe.Id, db);
                if (recipeFromDb != null)
                {
                    recipeFromDb.CoffeeType = coffeeFromDb;
                }
                else
                {
                    try
                    {
                        var newRecipe = await AddRecipeAsync(recipe);
                        newRecipe.CoffeeType = coffee;
                        recipe = newRecipe;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("AddRecipeToCoffee exception");
                        return new BlCallResult<RecipeDto>(BlCallResult.BlResult.RecipeError, e);
                    }

                }
                try
                {
                    db.SaveChanges();
                    Console.WriteLine("AddRecipeToCoffee exit");
                    return new BlCallResult<RecipeDto>(await GetRecipeAsync(recipe.Id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("AddRecipeToCoffee exception");
                    return new BlCallResult<RecipeDto>(BlCallResult.BlResult.DbError, e);
                }
            }
        }

        public async Task<BlCallResult<RecipeBookDto>> AddRecipeToRecipeBook(RecipeBookDto recipeBookDto, RecipeDto recipeDto)
        {
            Console.WriteLine("AddRecipeToRecipeBook enter");
            using (var db = new CoffeBookContext())
            {
                try
                {
                    recipeBookDto.Recipes.Add(recipeDto);
                    var updatedRecipeBook = await UpdateRecipeBookAsync(recipeBookDto);
                    recipeBookDto = updatedRecipeBook;
                }
                catch (Exception e)
                {
                    return new BlCallResult<RecipeBookDto>(BlCallResult.BlResult.RecipeBookError, e);
                    Console.WriteLine("AddRecipeToRecipeBook exception");
                    return new BlCallResult<RecipeBookDto>(BlCallResult.BlResult.RecipeBookError,e);
                }
                Console.WriteLine("AddRecipeToRecipeBook exit");
                return new BlCallResult<RecipeBookDto>(recipeBookDto);
            }
        }

        public async Task<BlCallResult<RecipeBookDto>> RemoveRecipeFromRecipeBook(RecipeBookDto recipeBookDto, RecipeDto recipeDto)
        {
            Console.WriteLine("RemoveRecipeFromRecipeBook enter");
            using (var db = new CoffeBookContext())
            {
                try
                {
                    recipeBookDto.Recipes.Remove(recipeDto);
                    var updatedRecipeBook = await UpdateRecipeBookAsync(recipeBookDto);
                    recipeBookDto = updatedRecipeBook;
                }
                catch (Exception e)
                {
                    Console.WriteLine("AddRecipeToRecipeBook exit");
                    return new BlCallResult<RecipeBookDto>(BlCallResult.BlResult.RecipeBookError, e);
                }
                Console.WriteLine("RemoveRecipeFromRecipeBook exit");
                return new BlCallResult<RecipeBookDto>(recipeBookDto);
            }
        }

        public async Task<BlCallResult<IList<CoffeeDto>>> SearchForCoffee(string searchTerm)
        {
            Console.WriteLine("SearchForCoffee enter");
            using (var db = new CoffeBookContext())
            {
                try
                {
                    var coffees = db.Coffes
                                  .Where(x => x.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));
                    var coffeeDtos = new List<CoffeeDto>();
                    foreach (var coffee in coffees)
                    {
                        coffeeDtos.Add(new CoffeeDto(coffee));
                    }
                    Console.WriteLine("SearchForCoffee enter");
                    return new BlCallResult<IList<CoffeeDto>>(coffeeDtos);
                }
                catch (Exception e)
                {
                    return new BlCallResult<IList<CoffeeDto>>(BlCallResult.BlResult.DbError, e);
                    Console.WriteLine("AddRecipeToRecipeBook exit");
                    return new BlCallResult<IList<CoffeeDto>>(BlCallResult.BlResult.DbError,e);
                }
            }
        }

        public async Task<BlCallResult<IList<RecipeDto>>> GetCoffeeRecipes(CoffeeDto coffeeDto)
        {
            Console.WriteLine("GetCoffeeRecipes enter");
            using (var db = new CoffeBookContext())
            {
                CoffeeDto coffee;
                try
                {
                    coffee = await GetCoffeeAsync(coffeeDto.Id);
                }
                catch (Exception e)
                {
                    return new BlCallResult<IList<RecipeDto>>(BlCallResult.BlResult.CoffeeError, e);
                }
                var recipes = db.Recipes.Include(x => x.CoffeeType).Where(x => x.CoffeeType.Id == coffee.Id);
                var recipeDtos = new List<RecipeDto>();
                foreach (var recipe in recipes)
                {
                    recipeDtos.Add(new RecipeDto(recipe));
                }
                Console.WriteLine("GetCoffeeRecipes exit");
                return new BlCallResult<IList<RecipeDto>>(recipeDtos);
            }
        }

        public async Task<CoffeeDto> AddCoffeeAsync(CoffeeDto newCoffee)
        {
            using (var db = new CoffeBookContext())
            {
                var coffeeEntity = ConvertDtoToEntity(newCoffee, db);
                return newCoffee;
            }
        }

        public async Task<CoffeeDto> GetCoffeeAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                return new CoffeeDto(db.Coffes.SingleOrDefault(x => x.Id == id));
            }
        }

        public async Task<IList<CoffeeDto>> GetAllCoffeesAsync()
        {
            using (var db = new CoffeBookContext())
            {
                var coffeeEntities = db.Coffes.ToList();
                var coffeeDtos = new List<CoffeeDto>();
                foreach (var entity in coffeeEntities)
                {
                    coffeeDtos.Add(new CoffeeDto(entity));
                }
                return coffeeDtos;
            }
        }

        public async Task<CoffeeDto> UpdateCoffeeAsync(CoffeeDto updatedCoffee)
        {
            using (var db = new CoffeBookContext())
            {
                var coffee = db.Coffes.SingleOrDefault(x => x.Id == updatedCoffee.Id);
                coffee.Name = updatedCoffee.Name;
                coffee.Picture = updatedCoffee.Picture;
                db.SaveChanges();
                return new CoffeeDto(coffee);
            }
        }

        public async void DeleteCoffeeAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                var toDelete = db.Coffes.SingleOrDefault(x => x.Id == id);
                db.Coffes.Remove(toDelete);
                db.SaveChanges();
            }
        }

        public async Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipe)
        {
            using (var db = new CoffeBookContext())
            {
                var entity = ConvertDtoToEntity(newRecipe, db);
                return new RecipeDto(entity);
            }
        }

        public async Task<RecipeDto> GetRecipeAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                return new RecipeDto(GetRecipe(id, db));
            }
        }

        public async Task<IList<RecipeDto>> GetAllRecipesAsync()
        {
            using (var db = new CoffeBookContext())
            {
                var entities = GetAllRecipes(db);
                var dtos = new List<RecipeDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(new RecipeDto(entity));
                }
                return dtos;
            }
        }

        public async Task<RecipeDto> UpdateRecipeAsync(RecipeDto updatedRecipe)
        {
            using (var db = new CoffeBookContext())
            {
                var recipeEntity = GetRecipe(updatedRecipe.Id, db);
                recipeEntity.Name = updatedRecipe.Name;
                recipeEntity.CoffeeType = ConvertDtoToEntity(updatedRecipe.CoffeeType, db);
                recipeEntity.Picture = updatedRecipe.Picture;
                recipeEntity.Description = updatedRecipe.Description;
                db.SaveChanges();
                return new RecipeDto(recipeEntity);
            }
        }

        public async void DeleteRecipeAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                var toDelete = db.Recipes.SingleOrDefault(x => x.Id == id);
                db.Recipes.Remove(toDelete);
                db.SaveChanges();
            }
        }

        public async Task<UserDto> AddUserAsync(UserDto newUser)
        {
            using (var db = new CoffeBookContext())
            {
                var userEntity = ConvertDtoToEntity(newUser, db);
                return newUser;
            }
        }

        public async Task<UserDto> GetUserAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                var user = GetUser(id, db);
                return user == null ? null : new UserDto(user);
            }
        }

        public async Task<UserDto> GetUserAsync(string name)
        {
            using (var db = new CoffeBookContext())
            {
                var user = GetUserByName(name, db);
                return user == null ? null : new UserDto(user);
            }
        }

        public async Task<IList<UserDto>> GetAllUsersAsync()
        {
            using (var db = new CoffeBookContext())
            {
                var entities = GetAllUsers(db);
                var dtos = new List<UserDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(new UserDto(entity));
                }
                return dtos;
            }
        }

        public async Task<UserDto> UpdateUserAsync(UserDto updatedUser)
        {
            using (var db = new CoffeBookContext())
            {
                var user = GetUser(updatedUser.Id, db);
                user.Name = updatedUser.Name;
                user.Password = updatedUser.Password;
                user.FbPassword = updatedUser.FbPassword;
                user.FbMail = updatedUser.FbMail;
                var recipeBooks = new List<RecipeBook>();
                foreach (var recipeBook in updatedUser.RecipeBooks)
                {
                    recipeBooks.Add(ConvertDtoToEntity(recipeBook, db));
                }
                user.RecipeBooks = recipeBooks;
                db.SaveChanges();
                return new UserDto(user);
            }
        }

        public async void DeleteUserAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                var toDelete = db.Users.Include(x => x.RecipeBooks).SingleOrDefault(x => x.Id == id);
                if (toDelete != null)
                {
                    foreach (var recipeBook in toDelete.RecipeBooks)
                    {
                        DeleteRecipeBookAsync(recipeBook.Id);
                    }
                }
                db.Users.Remove(toDelete);
                db.SaveChanges();
            }
        }

        public async Task<RecipeBookDto> AddRecipeBookAsync(RecipeBookDto newRecipeBook)
        {
            using (var db = new CoffeBookContext())
            {
                var recipeBook = ConvertDtoToEntity(newRecipeBook, db);
                return new RecipeBookDto(recipeBook);
            }
        }

        public async Task<RecipeBookDto> GetRecipeBookAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                return new RecipeBookDto(GetRecipeBook(id, db));
            }
        }

        public async Task<IList<RecipeBookDto>> GetAllRecipeBooksAsync()
        {
            using (var db = new CoffeBookContext())
            {
                var entities = GetAllRecipeBooks(db);
                var dtos = new List<RecipeBookDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(new RecipeBookDto(entity));
                }
                return dtos;
            }
        }

        public async Task<RecipeBookDto> UpdateRecipeBookAsync(RecipeBookDto updatedRecipeBook)
        {
            using (var db = new CoffeBookContext())
            {
                var recipeBook = ConvertDtoToEntity(updatedRecipeBook, db);
                recipeBook.Description = updatedRecipeBook.Description;
                recipeBook.Name = updatedRecipeBook.Name;
                var recipes = new List<Recipe>();
                foreach (var recipe in updatedRecipeBook.Recipes)
                {
                    recipes.Add(ConvertDtoToEntity(recipe, db));
                }
                recipeBook.Recipes = recipes;
                db.SaveChanges();
                return new RecipeBookDto(recipeBook);
            }
        }

        public async void DeleteRecipeBookAsync(long id)
        {
            using (var db = new CoffeBookContext())
            {
                var toDelete = GetRecipeBook(id, db);
                db.RecipeBooks.Remove(toDelete);
                db.SaveChanges();
            }
        }

        public async Task<bool> CoffeeExists(string name)
        {
            using (var db = new CoffeBookContext())
            {
                return db.Coffes.SingleOrDefault(x => x.Name.Equals(name)) == null;
            }
        }

        public async Task<bool> RecipeExists(string name)
        {
            using (var db = new CoffeBookContext())
            {
                return db.Recipes.SingleOrDefault(x => x.Name.Equals(name)) == null;
            }
        }

        private Coffee ConvertDtoToEntity(CoffeeDto coffeeDto, CoffeBookContext db)
        {
            var coffee = db.Coffes.SingleOrDefault(x => x.Id == coffeeDto.Id);
            if (coffee == null)
            {
                coffee = new Coffee
                {
                    Name = coffeeDto.Name,
                    Picture = coffeeDto.Picture
                };
                db.Coffes.Add(coffee);
                db.SaveChanges();
                coffeeDto.Id = coffee.Id;
            }
            return coffee;
        }

        private User ConvertDtoToEntity(UserDto userDto, CoffeBookContext db)
        {
            var user = GetUser(userDto.Id, db);
            if (user == null)
            {
                user = new User
                {
                    Name = userDto.Name,
                    Password = userDto.Password
                };
                var recipeBooks = new List<RecipeBook>();
                foreach (var recipeBook in userDto.RecipeBooks)
                {
                    recipeBooks.Add(ConvertDtoToEntity(recipeBook, db));
                }
                user.RecipeBooks = recipeBooks;
                db.Users.Add(user);
                db.SaveChanges();
                userDto.Id = user.Id;
            }
            return user;
        }

        private Recipe ConvertDtoToEntity(RecipeDto recipeDto, CoffeBookContext db)
        {
            var recipe = GetRecipe(recipeDto.Id, db);
            if (recipe == null)
            {
                recipe = new Recipe
                {
                    Name = recipeDto.Name,
                    Description = recipeDto.Description,
                    Picture = recipeDto.Picture,
                    CoffeeType = ConvertDtoToEntity(recipeDto.CoffeeType, db)
                };
                db.Recipes.Add(recipe);
                db.SaveChanges();
                recipeDto.Id = recipe.Id;
            }
            return recipe;
        }

        private RecipeBook ConvertDtoToEntity(RecipeBookDto recipeBookDto, CoffeBookContext db)
        {
            var recipeBook = GetRecipeBook(recipeBookDto.Id, db);
            if (recipeBook == null)
            {
                recipeBook = new RecipeBook
                {
                    Description = recipeBookDto.Description,
                    Name = recipeBookDto.Name
                };
                var recipes = new List<Recipe>();
                foreach (var recipe in recipeBookDto.Recipes)
                {
                    recipes.Add(ConvertDtoToEntity(recipe, db));
                }
                recipeBook.Recipes = recipes;
                db.RecipeBooks.Add(recipeBook);
                db.SaveChanges();
                recipeBookDto.Id = recipeBook.Id;
            }
            return recipeBook;
        }

        private RecipeBook GetRecipeBook(long id, CoffeBookContext db)
        {
            return id == -1 ? null : db.RecipeBooks.Include(x => x.Recipes)
                                     .Include(x => x.Recipes.Select(y => y.CoffeeType))
                                     .SingleOrDefault(x => x.Id == id);
        }

        private IList<RecipeBook> GetAllRecipeBooks(CoffeBookContext db)
        {
            return db.RecipeBooks.Include(x => x.Recipes)
                                 .Include(x => x.Recipes.Select(y => y.CoffeeType))
                                 .ToList();
        }

        private User GetUser(long id, CoffeBookContext db)
        {
            return id == -1 ? null : db.Users
                    .Include(x => x.RecipeBooks)
                    .Include(x => x.RecipeBooks.Select(y => y.Recipes))
                    .Include(x => x.RecipeBooks.Select(y => y.Recipes.Select(z => z.CoffeeType)))
                    .SingleOrDefault(x => x.Id == id);
        }
        private User GetUserByName(string name, CoffeBookContext db)
        {

            return db.Users
                .Include(x => x.RecipeBooks)
                .Include(x => x.RecipeBooks.Select(y => y.Recipes))
                .Include(x => x.RecipeBooks.Select(y => y.Recipes.Select(z => z.CoffeeType)))
                .SingleOrDefault(x => x.Name == name);
        }

        private IList<User> GetAllUsers(CoffeBookContext db)
        {
            return db.Users
                .Include(x => x.RecipeBooks)
                .Include(x => x.RecipeBooks.Select(y => y.Recipes))
                .Include(x => x.RecipeBooks.Select(y => y.Recipes.Select(z => z.CoffeeType))).ToList();
        }

        private Recipe GetRecipe(long id, CoffeBookContext db)
        {
            return id == -1 ? null : db.Recipes.Include(x => x.CoffeeType).SingleOrDefault(x => x.Id == id);
        }

        private IList<Recipe> GetAllRecipes(CoffeBookContext db)
        {
            return db.Recipes.Include(x => x.CoffeeType).ToList();
        }
        private void TestBL()
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();

            //Coffee
            var coffeeNew = new CoffeeDto { Name = "GoodCoffee" };
            var coffeeAdded = dbHandler.AddCoffeeAsync(coffeeNew).Result;
            var coffeeFromDb = dbHandler.GetCoffeeAsync(coffeeAdded.Id).Result;
            coffeeFromDb.Name = "Not GoodCoffee";
            var coffeeUpdated = dbHandler.UpdateCoffeeAsync(coffeeFromDb).Result;

            //Recipe
            var recipeNew = new RecipeDto
            {
                CoffeeType = coffeeNew,
                Description = "Very good recipe",
                Name = "Recipe for good coffee"
            };
            var recipeAdded = dbHandler.AddRecipeAsync(recipeNew).Result;
            var recipeFromDb = dbHandler.GetRecipeAsync(recipeAdded.Id).Result;
            recipeFromDb.Name = "Useless Recipe";
            var recipeUpdated = dbHandler.UpdateRecipeAsync(recipeFromDb).Result;

            //RecipeBook
            var recipeBookNew = new RecipeBookDto
            {
                Description = "RBook fro user Feri",
                Name = "FeriBook",
                Recipes = new List<RecipeDto> { recipeUpdated }
            };
            var recipeBookAdded = dbHandler.AddRecipeBookAsync(recipeBookNew).Result;
            var recipeBookFromDb = dbHandler.GetRecipeBookAsync(recipeBookAdded.Id).Result;
            recipeBookFromDb.Recipes.Add(new RecipeDto
            {
                CoffeeType = coffeeFromDb,
                Description = "Added Recipe",
                Name = "Added"
            });
            var recipeBookUpdated = dbHandler.UpdateRecipeBookAsync(recipeBookFromDb).Result;

            //User
            var userNew = new UserDto
            {
                RecipeBooks = new List<RecipeBookDto> { recipeBookUpdated },
                Name = "Feri",
                Password = "feriakia...",
                FbMail = "ez@ott.com",
                FbPassword = "aszadba..."
            };
            var userAdded = dbHandler.AddUserAsync(userNew).Result;
            var userFromDb = dbHandler.GetUserAsync(userAdded.Id).Result;
            userFromDb.FbPassword = "Teve";
            userFromDb.Name = "Galaktikus Johnny";
            userFromDb.RecipeBooks.RemoveAt(0);
            var userUpdated = dbHandler.UpdateUserAsync(userFromDb).Result;

            //GetAll
            var users = dbHandler.GetAllUsersAsync().Result;
            var rbs = dbHandler.GetAllRecipeBooksAsync().Result;
            var rs = dbHandler.GetAllRecipesAsync().Result;
            var cs = dbHandler.GetAllCoffeesAsync().Result;

            //Delete
            dbHandler.DeleteUserAsync(userNew.Id);
            dbHandler.DeleteRecipeBookAsync(recipeBookNew.Id);
            dbHandler.DeleteRecipeAsync(recipeNew.Id);
            dbHandler.DeleteCoffeeAsync(coffeeNew.Id);

            var usersafterDelete = dbHandler.GetAllUsersAsync().Result;
            var recipebooksafterDelete = dbHandler.GetAllRecipeBooksAsync().Result;
            var recipesafterDelete = dbHandler.GetAllRecipesAsync().Result;
            var coffeesafterDelete = dbHandler.GetAllCoffeesAsync().Result;
        }
    }
}
