using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Dtos.Recipes
{
    public class RecipeDetailDto : RecipeDto
    {
        public List<Step> Steps { get; set; } = new();
        public string CreatedBy { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
