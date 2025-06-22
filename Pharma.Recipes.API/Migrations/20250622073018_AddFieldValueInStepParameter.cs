using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharma.Recipes.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldValueInStepParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "StepParameters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "StepParameters");
        }
    }
}
