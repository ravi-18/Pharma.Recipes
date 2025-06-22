namespace Pharma.Recipes.API.Dtos.Recipes
{
    public class RecipeCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
