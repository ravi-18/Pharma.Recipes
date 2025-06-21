using Microsoft.EntityFrameworkCore;
using Pharma.Recipes.Models;

namespace Pharma.Recipes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Step> Steps => Set<Step>();
        public DbSet<StepParameter> StepParameters => Set<StepParameter>();
    }
}
