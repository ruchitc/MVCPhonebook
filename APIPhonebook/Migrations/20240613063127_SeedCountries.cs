using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhonebook.Migrations
{
    public partial class SeedCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                column: "CountryName",
                values: new object[]
                {
                    "India",
                    "United States",
                    "Canada",
                    "Australia",
                    "Germany",
                    "Japan"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValues: new object[]
                {
                    1, 2, 3, 4, 5, 6
                });
        }
    }
}
