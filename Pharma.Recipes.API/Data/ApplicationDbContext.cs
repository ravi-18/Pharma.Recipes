using Microsoft.EntityFrameworkCore;
using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId);
            modelBuilder.Entity<Step>()
                .HasMany(s => s.SubSteps)
                .WithOne(s => s.ParentStep)
                .HasForeignKey(s => s.ParentStepId);
            modelBuilder.Entity<StepParameter>()
                .HasOne(sp => sp.Step)
                .WithMany(s => s.Parameters)
                .HasForeignKey(sp => sp.StepId);

            modelBuilder
                .Entity<StepParameter>()
                .Property(p => p.Name)
                .HasConversion<string>();
        }

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Step> Steps => Set<Step>();
        public DbSet<StepParameter> StepParameters => Set<StepParameter>();
        public DbSet<User> Users => Set<User>();
    }
}
