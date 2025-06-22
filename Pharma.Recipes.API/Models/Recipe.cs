namespace Pharma.Recipes.API.Models
{
    public class Recipe : BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public List<Step> Steps { get; set; } = new();
    }
}
