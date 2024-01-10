using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDoLeoBlazor.API.Migrations
{
    /// <inheritdoc />
    public partial class Atualizacao1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "unidade",
                table: "Produto",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "senha",
                table: "Operador",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unidade",
                table: "Produto");

            migrationBuilder.AlterColumn<string>(
                name: "senha",
                table: "Operador",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
