namespace Pharma.Recipes.API.Dtos.Users
{
    public class ChangePasswordDto
    {
        public string Username { get; set; } = default!;
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
