using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeBook.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAs_IACategories_IACategoryId",
                table: "IAs");

            migrationBuilder.DropIndex(
                name: "IX_IAs_IACategoryId",
                table: "IAs");

            migrationBuilder.DropColumn(
                name: "IACategoryId",
                table: "IAs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "IAs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_IAs_CategoryId",
                table: "IAs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IAs_IACategories_CategoryId",
                table: "IAs",
                column: "CategoryId",
                principalTable: "IACategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IAs_IACategories_CategoryId",
                table: "IAs");

            migrationBuilder.DropIndex(
                name: "IX_IAs_CategoryId",
                table: "IAs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "IAs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IACategoryId",
                table: "IAs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IAs_IACategoryId",
                table: "IAs",
                column: "IACategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IAs_IACategories_IACategoryId",
                table: "IAs",
                column: "IACategoryId",
                principalTable: "IACategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
