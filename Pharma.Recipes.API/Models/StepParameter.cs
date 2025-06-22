using Pharma.Recipes.API.Enums;

namespace Pharma.Recipes.API.Models
{
    public class StepParameter : BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StepId { get; set; }

        public ParameterName Name { get; set; } = default!;
        public string DataType { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Description { get; set; }

        public Step Step { get; set; } = null!;
    }
}
