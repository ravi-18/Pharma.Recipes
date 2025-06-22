namespace Pharma.Recipes.API.Dtos.Steps
{
    public class StepDetailDto : StepDto
    {
        public string CreatedBy { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
