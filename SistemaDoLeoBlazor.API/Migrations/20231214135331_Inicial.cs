using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDoLeoBlazor.API.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    admin = table.Column<bool>(type: "bit", nullable: false),
                    inativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operador", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tela", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperadorTela",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    operadorId = table.Column<int>(type: "int", nullable: false),
                    telaId = table.Column<int>(type: "int", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    novo = table.Column<bool>(type: "bit", nullable: false),
                    editar = table.Column<bool>(type: "bit", nullable: false),
                    excluir = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperadorTela", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperadorTela_Operador_operadorId",
                        column: x => x.operadorId,
                        principalTable: "Operador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperadorTela_Tela_telaId",
                        column: x => x.telaId,
                        principalTable: "Tela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperadorTela_operadorId",
                table: "OperadorTela",
                column: "operadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperadorTela_telaId",
                table: "OperadorTela",
                column: "telaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperadorTela");

            migrationBuilder.DropTable(
                name: "Operador");

            migrationBuilder.DropTable(
                name: "Tela");
        }
    }
}
