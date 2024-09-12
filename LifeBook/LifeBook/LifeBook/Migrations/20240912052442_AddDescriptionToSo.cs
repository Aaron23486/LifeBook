using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeBook.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToSo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sos");
        }
    }
}
