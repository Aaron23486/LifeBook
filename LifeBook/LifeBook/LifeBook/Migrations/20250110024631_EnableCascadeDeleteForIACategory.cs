using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeBook.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteForIACategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAs_IACategories_CategoryId",
                table: "IAs");

            migrationBuilder.AddForeignKey(
                name: "FK_IAs_IACategories_CategoryId",
                table: "IAs",
                column: "CategoryId",
                principalTable: "IACategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAs_IACategories_CategoryId",
                table: "IAs");

            migrationBuilder.AddForeignKey(
                name: "FK_IAs_IACategories_CategoryId",
                table: "IAs",
                column: "CategoryId",
                principalTable: "IACategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
