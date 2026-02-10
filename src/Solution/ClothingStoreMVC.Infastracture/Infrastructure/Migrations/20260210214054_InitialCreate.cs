using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Store");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "Wishlists",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "UserAnswers",
                newName: "UserAnswers",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Styles",
                newName: "Styles",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Sizes",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Reviews",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                newName: "Quizzes",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Questions",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "ProductWishlist",
                newName: "ProductWishlist",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "ProductSizes",
                newName: "ProductSizes",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payments",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "OrderStatuses",
                newName: "OrderStatuses",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Orders",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItems",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Carts",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItems",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "AnswerStyles",
                newName: "AnswerStyles",
                newSchema: "Store");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answers",
                newSchema: "Store");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Wishlists",
                schema: "Store",
                newName: "Wishlists");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Store",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserAnswers",
                schema: "Store",
                newName: "UserAnswers");

            migrationBuilder.RenameTable(
                name: "Styles",
                schema: "Store",
                newName: "Styles");

            migrationBuilder.RenameTable(
                name: "Sizes",
                schema: "Store",
                newName: "Sizes");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Store",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Reviews",
                schema: "Store",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                schema: "Store",
                newName: "Quizzes");

            migrationBuilder.RenameTable(
                name: "Questions",
                schema: "Store",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "ProductWishlist",
                schema: "Store",
                newName: "ProductWishlist");

            migrationBuilder.RenameTable(
                name: "ProductSizes",
                schema: "Store",
                newName: "ProductSizes");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Store",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "Store",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "OrderStatuses",
                schema: "Store",
                newName: "OrderStatuses");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "Store",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "Store",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Store",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "Store",
                newName: "Carts");

            migrationBuilder.RenameTable(
                name: "CartItems",
                schema: "Store",
                newName: "CartItems");

            migrationBuilder.RenameTable(
                name: "AnswerStyles",
                schema: "Store",
                newName: "AnswerStyles");

            migrationBuilder.RenameTable(
                name: "Answers",
                schema: "Store",
                newName: "Answers");
        }
    }
}
