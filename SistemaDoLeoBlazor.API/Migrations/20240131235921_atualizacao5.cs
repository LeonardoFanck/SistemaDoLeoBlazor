using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDoLeoBlazor.API.Migrations
{
    /// <inheritdoc />
    public partial class atualizacao5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProximoRegistro",
                keyColumn: "id",
                keyValue: 1,
                column: "operador",
                value: 1);

            var command = @"create procedure atualizaEstoque(
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
            migrationBuilder.UpdateData(
                table: "ProximoRegistro",
                keyColumn: "id",
                keyValue: 1,
                column: "operador",
                value: 0);
        }
    }
}
