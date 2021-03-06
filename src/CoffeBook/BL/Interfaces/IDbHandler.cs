﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs;
using DB.Entities;

namespace BL.Interfaces
{
    interface IDbHandler
    {
        Task<BlCallResult<RecipeDto>> AddRecipeToCoffee(CoffeeDto coffee, RecipeDto recipe);
        Task<BlCallResult<RecipeBookDto>> AddRecipeToRecipeBook(RecipeBookDto recipeBook, RecipeDto recipe);
        Task<BlCallResult<RecipeBookDto>> RemoveRecipeFromRecipeBook(RecipeBookDto recipeBook, RecipeDto recipe);
        Task<BlCallResult<IList<CoffeeDto>>> SearchForCoffee(string searchTerm);
        Task<BlCallResult<IList<RecipeDto>>> GetCoffeeRecipes(CoffeeDto coffeeDto);

        Task<CoffeeDto> AddCoffeeAsync(CoffeeDto newCoffee);
        Task<CoffeeDto> GetCoffeeAsync(long id);
        Task<IList<CoffeeDto>> GetAllCoffeesAsync();
        Task<CoffeeDto> UpdateCoffeeAsync(CoffeeDto updatedCoffee);
        void DeleteCoffeeAsync(long id);

        Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipe);
        Task<RecipeDto> GetRecipeAsync(long id);
        Task<IList<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto> UpdateRecipeAsync(RecipeDto updatedRecipe);
        void DeleteRecipeAsync(long id);

        Task<UserDto> AddUserAsync(UserDto newUser);
        Task<UserDto> GetUserAsync(long id);
        Task<IList<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(UserDto updatedUser);
        void DeleteUserAsync(long id);

        Task<RecipeBookDto> AddRecipeBookAsync(RecipeBookDto newRecipeBook);
        Task<RecipeBookDto> GetRecipeBookAsync(long id);
        Task<IList<RecipeBookDto>> GetAllRecipeBooksAsync();
        Task<RecipeBookDto> UpdateRecipeBookAsync(RecipeBookDto updatedRecipeBook);
        void DeleteRecipeBookAsync(long id);

        Task<bool> CoffeeExists(string name);
        Task<bool> RecipeExists(string name);

    }
}
