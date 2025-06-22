using Pharma.Recipes.API.Dtos.Steps;
using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Repositories.Interfaces
{
    public interface IStepRepository
    {
        Task<bool> StepIsExist(Guid recipeId, Guid id);
        Task<IEnumerable<StepDto>> GetAllStepsAsync(Guid recipeId);
        Task<Step?> GetStepByIdAsync(Guid recipeId, Guid id);
        Task AddStepAsync(Step step);
        Task UpdateStepAsync(Step step);
        Task DeleteStepAsync(Step step);
    }
}
