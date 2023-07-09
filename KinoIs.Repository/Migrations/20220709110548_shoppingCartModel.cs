using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KinoIs.Repository.Migrations
{
    public partial class shoppingCartModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartId",
                table: "tickets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "shoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shoppingCarts_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ShoppingCartId",
                table: "tickets",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_OwnerId",
                table: "shoppingCarts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_shoppingCarts_ShoppingCartId",
                table: "tickets",
                column: "ShoppingCartId",
                principalTable: "shoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_shoppingCarts_ShoppingCartId",
                table: "tickets");

            migrationBuilder.DropTable(
                name: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_tickets_ShoppingCartId",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "tickets");
        }
    }
}
