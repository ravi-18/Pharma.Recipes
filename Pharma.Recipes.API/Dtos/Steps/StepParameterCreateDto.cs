using Pharma.Recipes.API.Enums;

namespace Pharma.Recipes.API.Dtos.Steps
{
    public class StepParameterCreateDto
    {
        public ParameterName Name { get; set; } = default!;
        public string DataType { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Description { get; set; }
    }
}
