using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDoLeoBlazor.API.Migrations
{
    /// <inheritdoc />
    public partial class atualizacao2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tipoOperacao",
                table: "Pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tipoOperacao",
                table: "Pedido");
        }
    }
}
