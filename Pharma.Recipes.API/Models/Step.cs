namespace Pharma.Recipes.API.Models
{
    public class Step : BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RecipeId { get; set; }
        public Guid? ParentStepId { get; set; }

        public string Title { get; set; } = default!;
        public int Sequence { get; set; }

        public Recipe Recipe { get; set; } = null!;
        public Step? ParentStep { get; set; }
        public List<Step> SubSteps { get; set; } = new();
        public virtual List<StepParameter> Parameters { get; set; } = new();
    }
}
