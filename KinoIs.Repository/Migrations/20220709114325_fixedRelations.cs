using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KinoIs.Repository.Migrations
{
    public partial class fixedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_shoppingCarts_ShoppingCartId",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_tickets_ShoppingCartId",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_OwnerId",
                table: "shoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "tickets");

            migrationBuilder.CreateTable(
                name: "TicketInShoppingCart",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TicketId = table.Column<Guid>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInShoppingCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInShoppingCart_shoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "shoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketInShoppingCart_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_OwnerId",
                table: "shoppingCarts",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCart_ShoppingCartId",
                table: "TicketInShoppingCart",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCart_TicketId",
                table: "TicketInShoppingCart",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketInShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_OwnerId",
                table: "shoppingCarts");

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartId",
                table: "tickets",
                type: "uniqueidentifier",
                nullable: true);

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
    }
}
