﻿namespace Pharma.Recipes.API.Dtos.Users
{
    public class RegisterUserDto
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
