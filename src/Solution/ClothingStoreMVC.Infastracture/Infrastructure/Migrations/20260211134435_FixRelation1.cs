using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Sizes_SizeId",
                schema: "Store",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Sizes_SizeId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                schema: "Store",
                table: "OrderItems",
                newName: "ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_SizeId",
                schema: "Store",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductSizeId");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                schema: "Store",
                table: "CartItems",
                newName: "ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_SizeId",
                schema: "Store",
                table: "CartItems",
                newName: "IX_CartItems_ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "CartItems",
                column: "ProductSizeId",
                principalSchema: "Store",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "OrderItems",
                column: "ProductSizeId",
                principalSchema: "Store",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "Store",
                table: "OrderItems",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ProductSizeId",
                schema: "Store",
                table: "OrderItems",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductSizeId",
                schema: "Store",
                table: "OrderItems",
                newName: "IX_OrderItems_SizeId");

            migrationBuilder.RenameColumn(
                name: "ProductSizeId",
                schema: "Store",
                table: "CartItems",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductSizeId",
                schema: "Store",
                table: "CartItems",
                newName: "IX_CartItems_SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Sizes_SizeId",
                schema: "Store",
                table: "CartItems",
                column: "SizeId",
                principalSchema: "Store",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "Store",
                table: "OrderItems",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Sizes_SizeId",
                schema: "Store",
                table: "OrderItems",
                column: "SizeId",
                principalSchema: "Store",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
