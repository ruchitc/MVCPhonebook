using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhonebook.Migrations
{
    public partial class AlterUsers_AddCheckConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_SecurityQuestionIds",
                table: "Users",
                sql: "SecurityQuestionId_1 != SecurityQuestionId_2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_SecurityQuestionIds",
                table: "Users");
        }
    }
}
