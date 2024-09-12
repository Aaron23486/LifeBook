using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeBook.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Atacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atacks_Sos_SoId",
                        column: x => x.SoId,
                        principalTable: "Sos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoId = table.Column<int>(type: "int", nullable: false),
                    AttackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tools_Atacks_AttackId",
                        column: x => x.AttackId,
                        principalTable: "Atacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tools_Sos_SoId",
                        column: x => x.SoId,
                        principalTable: "Sos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atacks_SoId",
                table: "Atacks",
                column: "SoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sos_Name",
                table: "Sos",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tools_AttackId",
                table: "Tools",
                column: "AttackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_SoId",
                table: "Tools",
                column: "SoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "Atacks");

            migrationBuilder.DropTable(
                name: "Sos");
        }
    }
}
