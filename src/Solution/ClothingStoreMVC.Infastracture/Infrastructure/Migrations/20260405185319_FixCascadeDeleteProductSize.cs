using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDeleteProductSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductId",
                schema: "Store",
                table: "ProductSizes");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "OrderItems",
                column: "ProductSizeId",
                principalSchema: "Store",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ProductId",
                schema: "Store",
                table: "ProductSizes",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductSizes_ProductSizeId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductId",
                schema: "Store",
                table: "ProductSizes");

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
                name: "FK_ProductSizes_Products_ProductId",
                schema: "Store",
                table: "ProductSizes",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
