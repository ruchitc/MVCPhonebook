using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhonebook.Migrations
{
    public partial class SeedSecurityQuestionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SecurityQuestions",
                column: "Question",
                values: new object[]
                {
                    "What was the name of your first pet?",
                    "Who was your childhood best friend?",
                    "Which is your favourite sports team?",
                    "What is your favourite hobby?",
                    "Who is your favourite author?",
                    "Which is your favourites movie?",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SecurityQuestions",
                keyColumn: "QuestionId",
                keyValues: new object[]
                {
                    1, 2, 3, 4, 5, 6
                });
        }
    }
}
