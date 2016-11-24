using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeBook.Model;
using BL;
using BL.DTOs;

namespace CoffeBook.Helpers
{
    public class UserHelper
    {
        public static async Task<User> GetUser(string name)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();

            var user = await dbHandler.GetUserAsync(name);

            return ConvertToUser(user);
        }

        public static async Task<User> RegisterUser(User user)
        {
            var dbHandler = new CoffeeBookDbHandlerFactory().GetDbHandler();

            UserDto regUser = ConvertFromUser(user);
            UserDto udto = await dbHandler.AddUserAsync(regUser);
            return ConvertToUser(udto);
        }

        private static User ConvertToUser(UserDto userDto)
        {
            User user = new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Password = userDto.Password,
                RecipeBooks = RecipeBookHelper.ConvertToRecipeBooks(userDto.RecipeBooks)
            };
            return user;
        }

        private static UserDto ConvertFromUser(User user)
        {
            UserDto userDto = new UserDto
            {
                Name = user.Name,
                Password = user.Password,
                RecipeBooks = RecipeBookHelper.ConvertFromRecipeBooks(user.RecipeBooks)
            };
            return userDto;
        }
    }
}
