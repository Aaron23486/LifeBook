using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeBook.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atacks_Sos_SoId",
                table: "Atacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Atacks_AttackId",
                table: "Tools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Atacks",
                table: "Atacks");

            migrationBuilder.RenameTable(
                name: "Atacks",
                newName: "Attacks");

            migrationBuilder.RenameIndex(
                name: "IX_Atacks_SoId",
                table: "Attacks",
                newName: "IX_Attacks_SoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attacks",
                table: "Attacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attacks_Sos_SoId",
                table: "Attacks",
                column: "SoId",
                principalTable: "Sos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Attacks_AttackId",
                table: "Tools",
                column: "AttackId",
                principalTable: "Attacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attacks_Sos_SoId",
                table: "Attacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Attacks_AttackId",
                table: "Tools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attacks",
                table: "Attacks");

            migrationBuilder.RenameTable(
                name: "Attacks",
                newName: "Atacks");

            migrationBuilder.RenameIndex(
                name: "IX_Attacks_SoId",
                table: "Atacks",
                newName: "IX_Atacks_SoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Atacks",
                table: "Atacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Atacks_Sos_SoId",
                table: "Atacks",
                column: "SoId",
                principalTable: "Sos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Atacks_AttackId",
                table: "Tools",
                column: "AttackId",
                principalTable: "Atacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
