namespace Pharma.Recipes.Models
{
    public class Recipe
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Step> Steps { get; set; } = new();
    }
}
