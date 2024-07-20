using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiklaYe.Migrations
{
    public partial class Deneme123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
