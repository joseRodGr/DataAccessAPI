using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessAPI.Migrations
{
    public partial class DescriptionAddedtoItemCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ItemCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ItemCategories");
        }
    }
}
