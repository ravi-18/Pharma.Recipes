using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Dtos.Steps
{
    public class StepDto
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid? ParentStepId { get; set; }

        public string Title { get; set; } = default!;
        public int Sequence { get; set; }
        public List<StepParameterDto> Parameters { get; set; } = new();
        public List<StepDto> SubSteps { get; set; } = new();
    }
}
