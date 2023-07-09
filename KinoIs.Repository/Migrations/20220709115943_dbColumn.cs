using Microsoft.EntityFrameworkCore.Migrations;

namespace KinoIs.Repository.Migrations
{
    public partial class dbColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketInShoppingCart_shoppingCarts_ShoppingCartId",
                table: "TicketInShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInShoppingCart_tickets_TicketId",
                table: "TicketInShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInShoppingCart",
                table: "TicketInShoppingCart");

            migrationBuilder.RenameTable(
                name: "TicketInShoppingCart",
                newName: "ticketInShoppingCarts");

            migrationBuilder.RenameIndex(
                name: "IX_TicketInShoppingCart_TicketId",
                table: "ticketInShoppingCarts",
                newName: "IX_ticketInShoppingCarts_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketInShoppingCart_ShoppingCartId",
                table: "ticketInShoppingCarts",
                newName: "IX_ticketInShoppingCarts_ShoppingCartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ticketInShoppingCarts",
                table: "ticketInShoppingCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ticketInShoppingCarts_shoppingCarts_ShoppingCartId",
                table: "ticketInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "shoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ticketInShoppingCarts_tickets_TicketId",
                table: "ticketInShoppingCarts",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ticketInShoppingCarts_shoppingCarts_ShoppingCartId",
                table: "ticketInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ticketInShoppingCarts_tickets_TicketId",
                table: "ticketInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ticketInShoppingCarts",
                table: "ticketInShoppingCarts");

            migrationBuilder.RenameTable(
                name: "ticketInShoppingCarts",
                newName: "TicketInShoppingCart");

            migrationBuilder.RenameIndex(
                name: "IX_ticketInShoppingCarts_TicketId",
                table: "TicketInShoppingCart",
                newName: "IX_TicketInShoppingCart_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_ticketInShoppingCarts_ShoppingCartId",
                table: "TicketInShoppingCart",
                newName: "IX_TicketInShoppingCart_ShoppingCartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInShoppingCart",
                table: "TicketInShoppingCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInShoppingCart_shoppingCarts_ShoppingCartId",
                table: "TicketInShoppingCart",
                column: "ShoppingCartId",
                principalTable: "shoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInShoppingCart_tickets_TicketId",
                table: "TicketInShoppingCart",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
