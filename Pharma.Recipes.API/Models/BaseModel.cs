namespace Pharma.Recipes.API.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; } = null;
    }
}
