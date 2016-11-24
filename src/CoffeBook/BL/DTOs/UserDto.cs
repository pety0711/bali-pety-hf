using System.Collections.Generic;
using DB.Entities;

namespace BL.DTOs
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string FbMail { get; set; }
        public string FbPassword { get; set; }
        public IList<RecipeBookDto> RecipeBooks { get; set; }

        public UserDto()
        {
            Id = -1;
        }

        public UserDto(User userEntity)
        {
            Id = userEntity.Id;
            Name = userEntity.Name;
            Password = userEntity.Password;
            FbMail = userEntity.FbMail;
            FbPassword = userEntity.FbPassword;
            RecipeBooks = new List<RecipeBookDto>();
            if (userEntity.RecipeBooks != null)
            {
                foreach (var item in userEntity.RecipeBooks)
                {
                    RecipeBooks.Add(new RecipeBookDto(item));
                }
            }
        }

    }
}
