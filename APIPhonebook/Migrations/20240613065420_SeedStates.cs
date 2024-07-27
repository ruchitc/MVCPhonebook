using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhonebook.Migrations
{
    public partial class SeedStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "StateName", "CountryId" },
                values: new object[,]
                {
                    { "Gujarat", 1 },
                    { "Maharashtra", 1 },
                    { "Rajasthan", 1 },
                    { "Punjab", 1 },
                    { "Alabama", 2 },
                    { "Arkansas", 2 },
                    { "Arizona", 2 },
                    { "Colorado", 2 },
                    { "Ontario", 3 },
                    { "Alberta", 3 },
                    { "Nova Scotia", 3 },
                    { "Saskatchewan", 3 },
                    { "New South Wales", 4 },
                    { "Queensland", 4 },
                    { "South Australia", 4 },
                    { "Tasmania", 4 },
                    { "Bavaria", 5 },
                    { "Berlin", 5 },
                    { "Hamburg", 5 },
                    { "Hesse", 5 },
                    { "Tokyo", 6 },
                    { "Akita", 6 },
                    { "Kyoto", 6 },
                    { "Osaka", 6 },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "States",
                keyColumn: "StateId",
                keyValues: new object[]
                {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24
                });
        }
    }
}
