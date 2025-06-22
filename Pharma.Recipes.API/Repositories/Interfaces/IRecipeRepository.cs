using Pharma.Recipes.API.Dtos.Recipes;
using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<Recipe?> GetRecipeByIdAsync(Guid id);
    }
}
