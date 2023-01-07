using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VOD.User.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "35fa4546-5646-40a8-97c2-b502c611eb5f", 0, "2b017452-05b6-4aeb-bbae-e14eb6f73610", "jane@vod.com", true, false, null, "JANE@VOD.COM", "JANE@VOD.COM", "AQAAAAIAAYagAAAAEL/6UfvlDrM4yvXW2LsK+KSGjZTNW5HQl/0MfZRPW3pAz7sxC3O0REXhbs8X7AKLeg==", null, false, "a5dbf3e6-e963-483d-babb-ce3ded568ee5", false, "jane@vod.com" },
                    { "f89bbddb-4cec-4510-9fc1-3a3da050d7f9", 0, "57579057-52b2-488c-920f-6be6b1d4003b", "john@vod.com", true, false, null, "JOHN@VOD.COM", "JOHN@VOD.COM", "AQAAAAIAAYagAAAAEIbHZv0Zdr5Ds9kkT54jn8skh/PZCc/sjQ73OmeXgXrwKKWYz/0Kbwjrw6ohqgku+Q==", null, false, "b8288291-48a7-4b37-90a7-03c77fa2cd68", false, "john@vod.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "35fa4546-5646-40a8-97c2-b502c611eb5f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f89bbddb-4cec-4510-9fc1-3a3da050d7f9");
        }
    }
}
