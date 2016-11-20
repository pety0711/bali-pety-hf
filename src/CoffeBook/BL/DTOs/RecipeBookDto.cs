using System.Collections.Generic;
using DB.Entities;

namespace BL.DTOs
{
    public class RecipeBookDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<RecipeDto> Recipes { get; set; }

        public RecipeBookDto()
        {
            Id = -1;
        }

        public RecipeBookDto(RecipeBook recipeBookEntity)
        {
            Id = recipeBookEntity.Id;
            Name = recipeBookEntity.Name;
            Description = recipeBookEntity.Description;
            Recipes = new List<RecipeDto>();
            foreach (var recipe in recipeBookEntity.Recipes)
            {
                Recipes.Add(new RecipeDto(recipe));
            }
        }
    }
}
