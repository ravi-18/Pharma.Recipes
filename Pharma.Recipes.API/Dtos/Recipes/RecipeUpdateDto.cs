using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Dtos.Recipes
{
    public class RecipeUpdateDto : RecipeCreateDto
    {
        public Guid Id { get; set; }
    }
}
