using Microsoft.EntityFrameworkCore.Migrations;

namespace SpacedRepApp.Infrastructure.Migrations
{
    public partial class repetitionCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepetitionCount",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepetitionCount",
                table: "Notes");
        }
    }
}
