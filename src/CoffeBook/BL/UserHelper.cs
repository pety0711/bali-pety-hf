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
                DB.Entities.User user 
                    = context.Users.Include("RecipeBooks")
                                    .Where(u => u.Name == name)
                                    .FirstOrDefault();
                return user == null ? null : convertUser(user);
            }
        }

        public static Model.User convertUser(DB.Entities.User user)
        {
            Model.User u = new Model.User
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                RecipeBooks = RecipeBookHelper.convertRecipeBooks(user.RecipeBooks)
            };
            return u;
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
