using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiklaYe_CQRS.Migrations
{
    public partial class UpdateBusinessOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfFileName",
                table: "BusinessOwners",
                newName: "PdfFilePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfFilePath",
                table: "BusinessOwners",
                newName: "PdfFileName");
        }
    }
}