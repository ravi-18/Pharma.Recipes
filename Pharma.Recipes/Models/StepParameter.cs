namespace Pharma.Recipes.Models
{
    public class StepParameter
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StepId { get; set; }

        public string Name { get; set; } = default!;
        public string DataType { get; set; } = default!;
        public string? Description { get; set; }

        public Step Step { get; set; } = null!;
    }
}
