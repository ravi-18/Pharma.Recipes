using Microsoft.EntityFrameworkCore;
using Pharma.Recipes.API.Data;
using Pharma.Recipes.API.Dtos.Recipes;
using Pharma.Recipes.API.Models;
using Pharma.Recipes.API.Repositories.Interfaces;

namespace Pharma.Recipes.API.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;
        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            return await _context.Recipes
                .Include(r => r.Steps)
                .Select(r => new RecipeDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                })
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(Guid id)
        {
            var recipe = await _context.Recipes
                .Where(r => r.Id == id)
                .Include(r => r.Steps)
                .ThenInclude(r => r.SubSteps)
                .ThenInclude(s => s.Parameters)
                .FirstOrDefaultAsync();
            return recipe;
        }
    }
}
