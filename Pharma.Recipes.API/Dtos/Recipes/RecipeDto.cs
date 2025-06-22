namespace Pharma.Recipes.API.Dtos.Recipes
{
    public class RecipeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
