using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Store",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Store",
                table: "Products");
        }
    }
}
