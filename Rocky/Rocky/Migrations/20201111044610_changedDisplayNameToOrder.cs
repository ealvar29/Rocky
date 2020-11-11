using Microsoft.EntityFrameworkCore.Migrations;

namespace Rocky.Migrations
{
    public partial class changedDisplayNameToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Category",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "DisplayName",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
