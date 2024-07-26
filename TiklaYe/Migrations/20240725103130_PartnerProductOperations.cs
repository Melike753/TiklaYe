using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiklaYe.Migrations
{
    public partial class PartnerProductOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_BusinessOwners_BusinessOwnerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BusinessOwnerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BusinessOwnerId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "PartnerProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessOwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerProducts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_PartnerProducts_BusinessOwners_BusinessOwnerId",
                        column: x => x.BusinessOwnerId,
                        principalTable: "BusinessOwners",
                        principalColumn: "BusinessOwnerId");
                    table.ForeignKey(
                        name: "FK_PartnerProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProducts_BusinessOwnerId",
                table: "PartnerProducts",
                column: "BusinessOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProducts_CategoryId",
                table: "PartnerProducts",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerProducts");

            migrationBuilder.AddColumn<int>(
                name: "BusinessOwnerId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BusinessOwnerId",
                table: "Products",
                column: "BusinessOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BusinessOwners_BusinessOwnerId",
                table: "Products",
                column: "BusinessOwnerId",
                principalTable: "BusinessOwners",
                principalColumn: "BusinessOwnerId");
        }
    }
}
