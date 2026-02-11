using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderId",
                schema: "Store",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                schema: "Store",
                table: "Payments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                schema: "Store",
                table: "Payments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeId",
                schema: "Store",
                table: "ProductSizes",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                schema: "Store",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                schema: "Store",
                table: "ProductSizes",
                column: "SizeId",
                principalSchema: "Store",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                schema: "Store",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_SizeId",
                schema: "Store",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderId",
                schema: "Store",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                schema: "Store",
                table: "Payments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                schema: "Store",
                table: "Payments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                schema: "Store",
                table: "Payments",
                column: "OrderId");
        }
    }
}
