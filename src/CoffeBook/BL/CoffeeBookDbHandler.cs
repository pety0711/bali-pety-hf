using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                        return new BlCallResult<RecipeDto>(BlCallResult.BlResult.CoffeeError, e);
                    }
                }
                var recipeFromDb = db.Recipes.SingleOrDefault(x => x.Id == recipe.Id);
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
                        return new BlCallResult<RecipeDto>(BlCallResult.BlResult.RecipeError, e);
                    }

                }
                try
                {
                    db.SaveChanges();
                    return new BlCallResult<RecipeDto>(await GetRecipeAsync(recipe.Id));
                }
                catch (Exception e)
                {
                    return new BlCallResult<RecipeDto>(BlCallResult.BlResult.DbError, e);
                }
            }
        }

        public async Task<BlCallResult<RecipeBookDto>> AddRecipeToRecipeBook(RecipeBookDto recipeBookDto, RecipeDto recipeDto)
        {
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
                    return new BlCallResult<RecipeBookDto>(BlCallResult.BlResult.RecipeBookError,e);
                }
                return new BlCallResult<RecipeBookDto>(recipeBookDto);
            }
        }

        public async Task<BlCallResult<RecipeBookDto>> RemoveRecipeFromRecipeBook(RecipeBookDto recipeBookDto, RecipeDto recipeDto)
        {
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
                    return new BlCallResult<RecipeBookDto>(BlCallResult.BlResult.RecipeBookError, e);
                }
                return new BlCallResult<RecipeBookDto>(recipeBookDto);
            }
        }

        public async Task<BlCallResult<IList<CoffeeDto>>> SearchForCoffee(string searchTerm)
        {
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
                    return new BlCallResult<IList<CoffeeDto>>(coffeeDtos);
                }
                catch (Exception e)
                {
                    return new BlCallResult<IList<CoffeeDto>>(BlCallResult.BlResult.DbError,e);
                }
            }
        }

        public async Task<BlCallResult<IList<RecipeDto>>> GetCoffeeRecipes(CoffeeDto coffeeDto)
        {
            using (var db = new CoffeBookContext())
            {
                CoffeeDto coffee;
                try
                {
                    coffee = await GetCoffeeAsync(coffeeDto.Id);
                }
                catch (Exception e)
                {
                    return new BlCallResult<IList<RecipeDto>>(BlCallResult.BlResult.CoffeeError,e);
                }
                var recipes = db.Recipes.Where(x => x.CoffeeType.Id == coffee.Id);
                var recipeDtos = new List<RecipeDto>();
                foreach (var recipe in recipes)
                {
                    recipeDtos.Add(new RecipeDto(recipe));
                }
                return new BlCallResult<IList<RecipeDto>>(recipeDtos);
            }
        }

        public async Task<CoffeeDto> AddCoffeeAsync(CoffeeDto newCoffee)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var coffeeEntity = ConvertDtoToEntity(newCoffee, db);
                    return newCoffee;
                }
            });
        }

        public async Task<CoffeeDto> GetCoffeeAsync(long id)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    return new CoffeeDto(db.Coffes.SingleOrDefault(x => x.Id == id));
                }
            });
        }

        public async Task<IList<CoffeeDto>> GetAllCoffeesAsync()
        {
            return await Task.Run(() =>
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
            });
        }

        public async Task<CoffeeDto> UpdateCoffeeAsync(CoffeeDto updatedCoffee)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var coffee = db.Coffes.SingleOrDefault(x => x.Id == updatedCoffee.Id);
                    coffee.Name = updatedCoffee.Name;
                    coffee.Picture = updatedCoffee.Picture;
                    db.SaveChanges();
                    return new CoffeeDto(coffee);
                }
            });
        }

        public async void DeleteCoffeeAsync(long id)
        {
            await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var toDelete = db.Coffes.SingleOrDefault(x => x.Id == id);
                    db.Coffes.Remove(toDelete);
                    db.SaveChanges();
                }
            });
        }

        public async Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipe)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var entity = ConvertDtoToEntity(newRecipe, db);
                    return newRecipe;
                }
            });
        }

        public async Task<RecipeDto> GetRecipeAsync(long id)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    return new RecipeDto(db.Recipes.SingleOrDefault(x => x.Id == id));
                }
            });
        }

        public async Task<IList<RecipeDto>> GetAllRecipesAsync()
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var entities = db.Recipes.ToList();
                    var dtos = new List<RecipeDto>();
                    foreach (var entity in entities)
                    {
                        dtos.Add(new RecipeDto(entity));
                    }
                    return dtos;
                }
            });
        }

        public async Task<RecipeDto> UpdateRecipeAsync(RecipeDto updatedRecipe)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var recipeEntity = db.Recipes.SingleOrDefault(x => x.Id == updatedRecipe.Id);
                    recipeEntity.Name = updatedRecipe.Name;
                    recipeEntity.CoffeeType = ConvertDtoToEntity(updatedRecipe.CoffeeType, db);
                    recipeEntity.Picture = updatedRecipe.Picture;
                    recipeEntity.Description = updatedRecipe.Description;
                    db.SaveChanges();
                    return new RecipeDto(recipeEntity);
                }
            });
        }

        public async void DeleteRecipesAsync(long id)
        {
            await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var toDelete = db.Recipes.SingleOrDefault(x => x.Id == id);
                    db.Recipes.Remove(toDelete);
                    db.SaveChanges();
                }
            });
        }

        public async Task<UserDto> AddUserAsync(UserDto newUser)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var userEntity = ConvertDtoToEntity(newUser, db);
                    return newUser;
                }
            });
        }

        public async Task<UserDto> GetUserAsync(long id)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    return new UserDto(db.Users.SingleOrDefault(x => x.Id == id));
                }
            });
        }

        public async Task<UserDto> GetUserAsync(string name)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    return new UserDto(db.Users.SingleOrDefault(x => x.Name == name));
                }
            });
        }

        public async Task<IList<UserDto>> GetAllUsersAsync()
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var entities = db.Users.ToList();
                    var dtos = new List<UserDto>();
                    foreach (var entity in entities)
                    {
                        dtos.Add(new UserDto(entity));
                    }
                    return dtos;
                }
            });
        }

        public async Task<UserDto> UpdateUserAsync(UserDto updatedUser)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var user = db.Users.SingleOrDefault(x => x.Id == updatedUser.Id);
                    user.Name = updatedUser.Name;
                    user.Password = updatedUser.Password;
                    var recipeBooks = new List<RecipeBook>();
                    foreach (var recipeBook in updatedUser.RecipeBooks)
                    {
                        recipeBooks.Add(ConvertDtoToEntity(recipeBook, db));
                    }
                    user.RecipeBooks = recipeBooks;
                    db.SaveChanges();
                    return new UserDto(user);
                }
            });
        }

        public async void DeleteUserAsync(long id)
        {
            await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var toDelete = db.Users.SingleOrDefault(x => x.Id == id);
                    db.Users.Remove(toDelete);
                    db.SaveChanges();
                }
            });
        }

        public async Task<RecipeBookDto> AddRecipeBookAsync(RecipeBookDto newRecipeBook)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var recipeBook = ConvertDtoToEntity(newRecipeBook, db);
                    return newRecipeBook;
                }
            });
        }

        public async Task<RecipeBookDto> GetRecipeBookAsync(long id)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    return new RecipeBookDto(db.RecipeBooks.SingleOrDefault(x => x.Id == id));
                }
            });
        }

        public async Task<IList<RecipeBookDto>> GetAllRecipeBooksAsync()
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var entities = db.RecipeBooks.ToList();
                    var dtos = new List<RecipeBookDto>();
                    foreach (var entity in entities)
                    {
                        dtos.Add(new RecipeBookDto(entity));
                    }
                    return dtos;
                }
            });
        }

        public async Task<RecipeBookDto> UpdateRecipeBookAsync(RecipeBookDto updatedRecipeBook)
        {
            return await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var recipeBook = ConvertDtoToEntity(updatedRecipeBook, db);
                    recipeBook.Description = updatedRecipeBook.Description;
                    recipeBook.Name = updatedRecipeBook.Name;
                    var recipes = new List<Recipe>();
                    foreach (var recipe in updatedRecipeBook.Recipes)
                    {
                        // ?? var updatedRecipe = UpdateRecipeAsync(recipe);
                        recipes.Add(ConvertDtoToEntity(recipe, db));
                    }
                    recipeBook.Recipes = recipes;
                    db.SaveChanges();
                    return new RecipeBookDto(recipeBook);
                }
            });
        }

        public async void DeleteRecipeBookAsync(long id)
        {
            await Task.Run(() =>
            {
                using (var db = new CoffeBookContext())
                {
                    var toDelete = db.RecipeBooks.SingleOrDefault(x => x.Id == id);
                    db.RecipeBooks.Remove(toDelete);
                    db.SaveChanges();
                }
            });
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
            var user = db.Users.SingleOrDefault(x => x.Id == userDto.Id);
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
            var recipe = db.Recipes.SingleOrDefault(x => x.Id == recipeDto.Id);
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
            var recipeBook = db.RecipeBooks.SingleOrDefault(x => x.Id == recipeBookDto.Id);
            if (recipeBook == null)
            {
                recipeBook = new RecipeBook();
                recipeBook.Description = recipeBookDto.Description;
                recipeBook.Name = recipeBookDto.Name;
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
    }
}
