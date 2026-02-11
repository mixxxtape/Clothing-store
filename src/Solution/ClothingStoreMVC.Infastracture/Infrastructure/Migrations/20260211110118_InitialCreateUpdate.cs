using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClothingStoreMVC.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                schema: "Store",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_Id",
                schema: "Store",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_Id",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                schema: "Store",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Answers_AnserId",
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

            migrationBuilder.DropColumn(
                name: "DefAdress",
                schema: "Store",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserAnswers",
                schema: "Store",
                newName: "UserAnswer",
                newSchema: "Store");

            migrationBuilder.RenameColumn(
                name: "AnserId",
                schema: "Store",
                table: "UserAnswer",
                newName: "AnswerId");

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
                name: "IX_UserAnswers_AnserId",
                schema: "Store",
                table: "UserAnswer",
                newName: "IX_UserAnswer_AnswerId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "Store",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Store",
                table: "Sizes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                schema: "Store",
                table: "Reviews",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                schema: "Store",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Store",
                table: "Payments",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Store",
                table: "OrderStatuses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ChangeReason",
                schema: "Store",
                table: "OrderStatuses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                schema: "Store",
                table: "Orders",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Store",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                schema: "Store",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                schema: "Store",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Store",
                table: "CartItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                schema: "Store",
                table: "CartItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                schema: "Store",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.CreateTable(
                name: "Results",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    QuizId = table.Column<int>(type: "integer", nullable: false),
                    StyleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalSchema: "Store",
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_Styles_StyleId",
                        column: x => x.StyleId,
                        principalSchema: "Store",
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Store",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "Store",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId1",
                schema: "Store",
                table: "OrderItems",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                schema: "Store",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerStyles_StyleId",
                schema: "Store",
                table: "AnswerStyles",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId1",
                schema: "Store",
                table: "Answers",
                column: "QuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Results_QuizId",
                schema: "Store",
                table: "Results",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_StyleId",
                schema: "Store",
                table: "Results",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_UserId",
                schema: "Store",
                table: "Results",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                schema: "Store",
                table: "Answers",
                column: "QuestionId",
                principalSchema: "Store",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_AnswerStyles_Styles_StyleId",
                schema: "Store",
                table: "AnswerStyles",
                column: "StyleId",
                principalSchema: "Store",
                principalTable: "Styles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                schema: "Store",
                table: "CartItems",
                column: "CartId",
                principalSchema: "Store",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "Store",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "Store",
                principalTable: "Orders",
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
                name: "FK_Questions_Quizzes_QuizId",
                schema: "Store",
                table: "Questions",
                column: "QuizId",
                principalSchema: "Store",
                principalTable: "Quizzes",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                schema: "Store",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId1",
                schema: "Store",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerStyles_Styles_StyleId",
                schema: "Store",
                table: "AnswerStyles");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                schema: "Store",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId1",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                schema: "Store",
                table: "Questions");

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

            migrationBuilder.DropTable(
                name: "Results",
                schema: "Store");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId1",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId",
                schema: "Store",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_AnswerStyles_StyleId",
                schema: "Store",
                table: "AnswerStyles");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId1",
                schema: "Store",
                table: "Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswer",
                schema: "Store",
                table: "UserAnswer");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                schema: "Store",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CartId",
                schema: "Store",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                schema: "Store",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "UserAnswer",
                schema: "Store",
                newName: "UserAnswers",
                newSchema: "Store");

            migrationBuilder.RenameColumn(
                name: "AnswerId",
                schema: "Store",
                table: "UserAnswers",
                newName: "AnserId");

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
                newName: "IX_UserAnswers_AnserId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "Store",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefAdress",
                schema: "Store",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Store",
                table: "Sizes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                schema: "Store",
                table: "Reviews",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                schema: "Store",
                table: "Questions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                schema: "Store",
                table: "Payments",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Store",
                table: "OrderStatuses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ChangeReason",
                schema: "Store",
                table: "OrderStatuses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                schema: "Store",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Store",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Store",
                table: "CartItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                schema: "Store",
                table: "Answers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                schema: "Store",
                table: "UserAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                schema: "Store",
                table: "Answers",
                column: "QuestionId",
                principalSchema: "Store",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_Id",
                schema: "Store",
                table: "CartItems",
                column: "Id",
                principalSchema: "Store",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_Id",
                schema: "Store",
                table: "OrderItems",
                column: "Id",
                principalSchema: "Store",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                schema: "Store",
                table: "Questions",
                column: "QuizId",
                principalSchema: "Store",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Answers_AnserId",
                schema: "Store",
                table: "UserAnswers",
                column: "AnserId",
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
    }
}
