using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaDoLeoBlazor.API.Migrations
{
    /// <inheritdoc />
    public partial class atualizacao4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OperadorTela",
                columns: new[] { "id", "ativo", "editar", "excluir", "novo", "operadorId", "telaId" },
                values: new object[,]
                {
                    { 1, true, true, true, true, 1, 1 },
                    { 2, true, true, true, true, 1, 2 },
                    { 3, true, true, true, true, 1, 3 },
                    { 4, true, true, true, true, 1, 4 },
                    { 5, true, true, true, true, 1, 5 },
                    { 6, true, true, true, true, 1, 6 }
                });

            var command = @"create procedure teste(
	                            @id int)
                            as
                            begin
	
	                            declare @venda bigint;
	                            declare @compra bigint;
	                            declare @total bigint;

	                            select @venda = ISNULL(SUM(PedidoItem.quantidade), 0) FROM PedidoItem
	                            join Pedido on PedidoItem.pedidoId = Pedido.id
	                            where PedidoItem.produtoId = @id and Pedido.tipoOperacao like 'Venda';

	                            select @compra = ISNULL(SUM(PedidoItem.quantidade), 0) FROM PedidoItem
	                            join Pedido on PedidoItem.pedidoId = Pedido.id
	                            where PedidoItem.produtoId = @id and Pedido.tipoOperacao like 'Compra';

	                            set @total = @compra - @venda;

	                            update Produto set estoque = @total where id = @id;
                            end";

            migrationBuilder.Sql(command);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OperadorTela",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OperadorTela",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OperadorTela",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OperadorTela",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OperadorTela",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OperadorTela",
                keyColumn: "id",
                keyValue: 6);
        }
    }
}
