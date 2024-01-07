using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingPedido
    {
        public static PedidoDTO PedidoToDto(this Pedido pedido)
        {
            return new PedidoDTO
            {
                id = pedido.id,
                clienteId = pedido.clienteId,
                clienteNome = pedido.cliente.nome,
                data = pedido.data,
                formaPgtoId = pedido.formaPgtoId,
                formaPgtoNome = pedido.formaPgto.nome,
                desconto = pedido.desconto,
                total = pedido.total,
                valor = pedido.valor
            };
        }

        public static IEnumerable<PedidoDTO> PedidosToDto(this IEnumerable<Pedido> pedidos)
        {
            return (from pedido in pedidos
                    select new PedidoDTO
                    {
                        id = pedido.id,
                        clienteId = pedido.clienteId,
                        clienteNome = pedido.cliente.nome,
                        data = pedido.data,
                        formaPgtoId = pedido.formaPgtoId,
                        formaPgtoNome = pedido.formaPgto.nome,
                        desconto = pedido.desconto,
                        total = pedido.total,
                        valor = pedido.valor
                    });
        }
    }
}
