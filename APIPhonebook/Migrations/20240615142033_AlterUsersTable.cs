using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhonebook.Migrations
{
    public partial class AlterUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "SecurityAnswerHash_1",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "SecurityAnswerHash_2",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "SecurityAnswerSalt_1",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "SecurityAnswerSalt_2",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "SecurityQuestionId_1",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecurityQuestionId_2",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContactNumber",
                table: "Users",
                column: "ContactNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LoginId",
                table: "Users",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SecurityQuestionId_1",
                table: "Users",
                column: "SecurityQuestionId_1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SecurityQuestionId_2",
                table: "Users",
                column: "SecurityQuestionId_2");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SecurityQuestions_SecurityQuestionId_1",
                table: "Users",
                column: "SecurityQuestionId_1",
                principalTable: "SecurityQuestions",
                principalColumn: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SecurityQuestions_SecurityQuestionId_2",
                table: "Users",
                column: "SecurityQuestionId_2",
                principalTable: "SecurityQuestions",
                principalColumn: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SecurityQuestions_SecurityQuestionId_1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SecurityQuestions_SecurityQuestionId_2",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ContactNumber",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LoginId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SecurityQuestionId_1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SecurityQuestionId_2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityAnswerHash_1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityAnswerHash_2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityAnswerSalt_1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityAnswerSalt_2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityQuestionId_1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityQuestionId_2",
                table: "Users");
        }
    }
}
