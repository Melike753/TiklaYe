using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiklaYe_CQRS.Migrations
{
    public partial class UpdatePurchasesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessOwnerId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BusinessOwnerId",
                table: "Purchases",
                column: "BusinessOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_BusinessOwners_BusinessOwnerId",
                table: "Purchases",
                column: "BusinessOwnerId",
                principalTable: "BusinessOwners",
                principalColumn: "BusinessOwnerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_BusinessOwners_BusinessOwnerId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_BusinessOwnerId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "BusinessOwnerId",
                table: "Purchases");
        }
    }
}
