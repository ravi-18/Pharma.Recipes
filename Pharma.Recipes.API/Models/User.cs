using Pharma.Recipes.API.Enums;

namespace Pharma.Recipes.API.Models
{
    public class User: BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = Constanta.Operator; // "Admin" / "Operator"
    }

}
