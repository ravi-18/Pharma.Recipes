using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharma.Recipes.API.Data;
using Pharma.Recipes.API.Dtos.Recipes;
using Pharma.Recipes.API.Models;
using Pharma.Recipes.API.Repositories.Interfaces;

namespace Pharma.Recipes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRecipeRepository _recipeRepository;

        public RecipeController(ApplicationDbContext context, IRecipeRepository recipeRepository)
        {
            _context = context;
            _recipeRepository = recipeRepository;
        }

        // GET: api/Recipe
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
        {
            var recipes = await _recipeRepository.GetAllRecipesAsync();
            return Ok(recipes);
        }

        // GET: api/Recipe/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Recipe>> GetRecipe(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError("Id", "ID cannot be empty.");
                return BadRequest(ModelState);
            }

            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Recipe/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutRecipe(Guid id, RecipeUpdateDto recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.Id)
            {
                ModelState.AddModelError("Id", "ID mismatch in request body and URL.");
                return BadRequest(ModelState);
            }

            var recipeForUpdate = await _context.Recipes.FindAsync(id);

            if (recipeForUpdate == null)
            {
                return NotFound();
            }

            var trx = await _context.Database.BeginTransactionAsync();

            try
            {
                recipeForUpdate.Name = recipe.Name;
                recipeForUpdate.Description = recipe.Description;
                recipeForUpdate.ModifiedBy = "Admin"; // Assuming you have a way to set the updated by field
                recipeForUpdate.ModifiedAt = DateTime.UtcNow; // Assuming you have a way to set the updated at field

                _context.Entry(recipeForUpdate).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipe
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Recipe>> PostRecipe(RecipeCreateDto recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(recipe.Name))
            {
                ModelState.AddModelError("Name", "Recipe name is required.");
                return BadRequest(ModelState);
            }

            var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                var newRecipe = new Recipe
                {
                    Name = recipe.Name,
                    Description = recipe.Description,
                    CreatedBy = "Admin"
                };

                _context.Recipes.Add(newRecipe);
                await _context.SaveChangesAsync();
                await trx.CommitAsync();

                return CreatedAtAction("GetRecipe", new { id = newRecipe.Id }, newRecipe);
            }
            catch (Exception ex)
            {
                if (trx != null)
                {
                    await trx.RollbackAsync();
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
