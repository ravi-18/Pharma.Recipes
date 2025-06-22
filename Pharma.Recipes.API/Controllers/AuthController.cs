using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pharma.Recipes.API.Data;
using Pharma.Recipes.API.Dtos.Users;
using Pharma.Recipes.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pharma.Recipes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return BadRequest(ModelState); 
            }

            if (string.IsNullOrEmpty(dto.Username))
            {
                ModelState.AddModelError("Username", "Username is required");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(dto.Email))
            {
                ModelState.AddModelError("Email", "Email is required");
                return BadRequest(ModelState);
            }

            // Validate email format using System.Net.Mail
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(dto.Email);
            }
            catch
            {
                ModelState.AddModelError("Email", "Invalid email format");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(dto.Password) || dto.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters long");
                return BadRequest(ModelState);
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedBy = dto.Username,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return NotFound("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                return BadRequest("Current password is incorrect");

            var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                var username = User.Identity?.Name;
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                user.ModifiedBy = username;

                await _context.SaveChangesAsync();
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                await trx.RollbackAsync();

                ModelState.AddModelError("ChangePassword", ex.Message);
                return BadRequest(ModelState);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
