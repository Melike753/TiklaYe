using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiklaYe.Migrations
{
    public partial class UpdateBusinessOwners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "BusinessOwners",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "BusinessOwners");
        }
    }
}
