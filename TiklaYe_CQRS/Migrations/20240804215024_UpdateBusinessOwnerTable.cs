using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiklaYe_CQRS.Migrations
{
    public partial class UpdateBusinessOwnerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusinessOwners",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusinessOwners");
        }
    }
}
