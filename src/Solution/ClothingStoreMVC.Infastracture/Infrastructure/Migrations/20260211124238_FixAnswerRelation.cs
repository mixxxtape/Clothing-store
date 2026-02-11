using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAnswerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId1",
                schema: "Store",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId1",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_Answers_AnswerId",
                schema: "Store",
                table: "UserAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_Questions_QuestionId",
                schema: "Store",
                table: "UserAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_Users_UserId",
                schema: "Store",
                table: "UserAnswer");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId1",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId1",
                schema: "Store",
                table: "Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswer",
                schema: "Store",
                table: "UserAnswer");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                schema: "Store",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "UserAnswer",
                schema: "Store",
                newName: "UserAnswers",
                newSchema: "Store");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswer_UserId",
                schema: "Store",
                table: "UserAnswers",
                newName: "IX_UserAnswers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswer_QuestionId",
                schema: "Store",
                table: "UserAnswers",
                newName: "IX_UserAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswer_AnswerId",
                schema: "Store",
                table: "UserAnswers",
                newName: "IX_UserAnswers_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                schema: "Store",
                table: "UserAnswers",
                column: "Id");

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
                name: "FK_UserAnswers_Answers_AnswerId",
                schema: "Store",
                table: "UserAnswers",
                column: "AnswerId",
                principalSchema: "Store",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                schema: "Store",
                table: "UserAnswers",
                column: "QuestionId",
                principalSchema: "Store",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Users_UserId",
                schema: "Store",
                table: "UserAnswers",
                column: "UserId",
                principalSchema: "Store",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Answers_AnswerId",
                schema: "Store",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                schema: "Store",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Users_UserId",
                schema: "Store",
                table: "UserAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswers",
                schema: "Store",
                table: "UserAnswers");

            migrationBuilder.RenameTable(
                name: "UserAnswers",
                schema: "Store",
                newName: "UserAnswer",
                newSchema: "Store");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_UserId",
                schema: "Store",
                table: "UserAnswer",
                newName: "IX_UserAnswer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_QuestionId",
                schema: "Store",
                table: "UserAnswer",
                newName: "IX_UserAnswer_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_AnswerId",
                schema: "Store",
                table: "UserAnswer",
                newName: "IX_UserAnswer_AnswerId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                schema: "Store",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId1",
                schema: "Store",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswer",
                schema: "Store",
                table: "UserAnswer",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId1",
                schema: "Store",
                table: "OrderItems",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId1",
                schema: "Store",
                table: "Answers",
                column: "QuestionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId1",
                schema: "Store",
                table: "Answers",
                column: "QuestionId1",
                principalSchema: "Store",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId1",
                schema: "Store",
                table: "OrderItems",
                column: "OrderId1",
                principalSchema: "Store",
                principalTable: "Orders",
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

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_Answers_AnswerId",
                schema: "Store",
                table: "UserAnswer",
                column: "AnswerId",
                principalSchema: "Store",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_Questions_QuestionId",
                schema: "Store",
                table: "UserAnswer",
                column: "QuestionId",
                principalSchema: "Store",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_Users_UserId",
                schema: "Store",
                table: "UserAnswer",
                column: "UserId",
                principalSchema: "Store",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
