using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pharma.Recipes.API.Data;
using Pharma.Recipes.API.Dtos.Steps;
using Pharma.Recipes.API.Models;
using Pharma.Recipes.API.Repositories.Interfaces;

namespace Pharma.Recipes.API.Repositories
{
    public class StepRepository : IStepRepository
    {
        private readonly ApplicationDbContext _context;
        public StepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> StepIsExist(Guid recipeId, Guid id)
        {
            return await _context.Steps.AnyAsync(s => s.RecipeId == recipeId && s.Id == id);
        }

        public async Task<IEnumerable<StepDto>> GetAllStepsAsync(Guid recipeId)
        {
            var steps = await _context.Steps
                .Include(s => s.Parameters)
                .Include(p => p.Recipe)
                .Include(s => s.SubSteps)
                .Where(s => s.RecipeId == recipeId)
                .ToListAsync();

            var stepTree = BuildStepTree(steps);
            return stepTree;
        }

        public static List<StepDto> BuildStepTree(List<Step> allSteps, Guid? parentId = null)
        {
            return allSteps
                .Where(s => s.ParentStepId == parentId)
                .OrderBy(s => s.Sequence)
                .Select(s => new StepDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    RecipeId = s.RecipeId,
                    ParentStepId = s.ParentStepId,
                    Sequence = s.Sequence,
                    Parameters = s.Parameters.Select(p => new StepParameterDto
                    {
                        StepId = p.StepId,
                        Name = p.Name,
                        DataType = p.DataType,
                        Value = p.Value,
                        Description = p.Description,
                    }).ToList(),
                    SubSteps = BuildStepTree(allSteps, s.Id),// rekursif
                })  
                .ToList();
        }

        public async Task<StepDetailDto?> GetStepByIdAsync(Guid recipeId, Guid id)
        {
            return await _context.Steps
                .Include(s => s.Parameters) // Include parameters if needed
                .Include(s => s.SubSteps) // Include sub-steps if needed
                .Where(s => s.RecipeId == recipeId && s.Id == id)
                .Select(s => new StepDetailDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    RecipeId = s.RecipeId,
                    ParentStepId = s.ParentStepId,
                    Sequence = s.Sequence,
                    Parameters = s.Parameters.Select(p => new StepParameterDto
                    {
                        StepId = p.StepId,
                        Name = p.Name,
                        DataType = p.DataType,
                        Value = p.Value,
                        Description = p.Description
                    }).ToList(),
                    SubSteps = BuildStepTree(_context.Steps.ToList(), s.Id), // Get sub-steps recursively
                    CreatedAt = s.CreatedAt,
                    CreatedBy = s.CreatedBy,
                    ModifiedAt = s.ModifiedAt,
                    ModifiedBy = s.ModifiedBy,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Step?> GetStepByIdModelAsync(Guid recipeId, Guid id)
        {
            return await _context.Steps.FirstOrDefaultAsync(e => e.Id == id && e.RecipeId == recipeId);
        }

        public async Task AddStepAsync(Step step)
        {
            _context.Steps.Add(step);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStepAsync(Step step)
        {
            _context.Entry(step).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStepAsync(Step step)
        {
            _context.Steps.Remove(step);
            await _context.SaveChangesAsync();
        }
    }
}
