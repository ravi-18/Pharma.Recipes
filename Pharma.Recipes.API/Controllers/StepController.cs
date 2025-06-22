using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.Recipes.API.Data;
using Pharma.Recipes.API.Dtos.Steps;
using Pharma.Recipes.API.Models;
using Pharma.Recipes.API.Repositories.Interfaces;

namespace Pharma.Recipes.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class StepController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStepRepository _stepRepository;
        private readonly IMapper _mapper;

        public StepController(IStepRepository stepRepository, ApplicationDbContext context, IMapper mapper)
        {
            _stepRepository = stepRepository;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Step
        [HttpGet("api/recipes/{recipeId}/step")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Step>>> GetSteps(Guid recipeId)
        {
            return Ok(await _stepRepository.GetAllStepsAsync(recipeId));
        }

        // GET: api/Step/5
        [HttpGet("api/recipes/{recipeId}/step/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Step>> GetStep(Guid recipeId, Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError("Id", "ID cannot be empty.");
                return BadRequest(ModelState);
            }

            var step = await _stepRepository.GetStepByIdAsync(recipeId, id);

            if (step == null)
            {
                return NotFound();
            }

            return step;
        }

        // PUT: api/Step/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/recipe/{recipeId}/step/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutStep(Guid recipeId, Guid id, Step step)
        {
            if (id != step.Id)
            {
                return BadRequest();
            }

            var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _stepRepository.UpdateStepAsync(step);
                await trx.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _stepRepository.StepIsExist(recipeId, id))
                {
                    if (trx != null)
                        await trx.RollbackAsync();
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Step
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/recipe/{recipeId}/step")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Step>> PostStep(Guid recipeId, StepCreateDto step)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            
            var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                var stepId = Guid.NewGuid();

                var stepEntity = new Step()
                {
                    Id = stepId,
                    RecipeId = step.RecipeId,
                    Title = step.Title,
                    Sequence = step.Sequence,
                    Parameters = step.Parameters.Select(e => new StepParameter()
                    {
                        StepId = stepId,
                        Name = e.Name,
                        DataType = e.DataType,
                        Value = e.Value,
                        Description = e.Description,
                        CreatedBy = "Admin",
                    }).ToList(),
                    CreatedBy = "Admin",
                };

                await _stepRepository.AddStepAsync(stepEntity);
                await trx.CommitAsync();
                return CreatedAtAction("GetStep", new { id = stepEntity.Id }, stepEntity);
            }
            catch (Exception ex)
            {
                if (trx != null)
                    await trx.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Step/5
        [HttpDelete("api/recipe/{recipeId}/step/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteStep(Guid recipeId, Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError("Id", "ID cannot be empty.");
                return BadRequest(ModelState);
            }

            if (!await _stepRepository.StepIsExist(recipeId, id))
            {
                return NotFound();
            }

            var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                var step = await _stepRepository.GetStepByIdAsync(recipeId, id);
                if (step == null)
                {
                    return NotFound();
                }

                await _stepRepository.DeleteStepAsync(step);
                await trx.CommitAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                if (trx != null)
                    await trx.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }
}
