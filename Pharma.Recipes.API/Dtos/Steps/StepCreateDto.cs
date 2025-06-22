namespace Pharma.Recipes.API.Dtos.Steps
{
    public class StepCreateDto
    {
        public Guid RecipeId { get; set; }
        public Guid? ParentStepId { get; set; }

        public string Title { get; set; } = default!;
        public int Sequence { get; set; }
        public List<StepParameterCreateDto> Parameters { get; set; } = new();
    }
}
