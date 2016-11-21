using DB.Entities;

namespace BL.DTOs
{
    public class RecipeDto
    {
        public long Id { get; set; }
        public CoffeeDto CoffeeType { get; set; }
        public byte[] Picture { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public RecipeDto()
        {
            Id = -1;
        }

        public RecipeDto(Recipe recipeEntity)
        {
            Id = recipeEntity.Id;
            CoffeeType = recipeEntity.CoffeeType == null ? null : new CoffeeDto(recipeEntity.CoffeeType);
            Picture = recipeEntity.Picture;
            Name = recipeEntity.Name;
            Description = recipeEntity.Description;
        }
    }
}
