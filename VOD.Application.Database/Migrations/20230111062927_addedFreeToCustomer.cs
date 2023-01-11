using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOD.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedFreeToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Free",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Free",
                table: "Courses");
        }
    }
}
