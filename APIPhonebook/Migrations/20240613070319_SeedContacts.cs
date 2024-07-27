using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhonebook.Migrations
{
    public partial class SeedContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "FirstName", "LastName", "ContactNumber", "Gender", "CountryId", "StateId", "IsFavourite", "Email", "Company" },
                values: new object[,]
                {
                    { "John", "Doe", "1234567890", "M", 1, 1, true, "john.doe@example.com", "XYZ Corp" },
                    { "Jane", "Smith", "1234567891", "F", 2, 5, true, "jane.smith@example.com", "XYZ Corp" },
                    { "Michael", "Johnson", "1234567892", "M", 3, 9, false, "michael.johnson@example.com", "Smith & Sons" },
                    { "Emily", "Brown", "1234567893", "F", 4, 13, true, "emily.brown@example.com", "Smith & Sons" },
                    { "David", "Wilson", "1234567894", "M", 5, 17, false, "david.wilson@example.com", "123 Enterprises" },
                    { "Jessica", "Martinez", "1234567895", "F", 6, 21, false, "jessica.martinez@example.com", "123 Enterprises" },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "ContactId",
                keyValues: new object[]
                {
                    1, 2, 3, 4, 5, 6
                });
        }
    }
}
