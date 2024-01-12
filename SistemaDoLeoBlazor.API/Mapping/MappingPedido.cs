using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingPedido
    {
        public static PedidoDTO PedidoToDtoGet(this Pedido pedido)
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
                valor = pedido.valor,
                tipoOperacao = pedido.tipoOperacao
            };
        }

        public static PedidoDTO PedidoToDtoSet(this Pedido pedido)
        {
            return new PedidoDTO
            {
                id = pedido.id,
                clienteId = pedido.clienteId,
                data = pedido.data,
                formaPgtoId = pedido.formaPgtoId,
                desconto = pedido.desconto,
                total = pedido.total,
                valor = pedido.valor,
                tipoOperacao = pedido.tipoOperacao
            };
        }

        // ITENS

        public static IEnumerable<PedidoDTO> PedidosToDtoGet(this IEnumerable<Pedido> pedidos)
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
                        valor = pedido.valor,
                        tipoOperacao = pedido.tipoOperacao
                    });
        }

        public static PedidoItemDTO PedidoItemToDtoGet(this PedidoItem item)
        {
            return new PedidoItemDTO
            {
                id = item.id,
                desconto = item.desconto,
                pedidoId = item.pedidoId,
                total = item.total,
                produtoId = item.produtoId,
                produtoNome = item.produto.nome,
                valor = item.valor,
                quantidade = item.quantidade
            };
        }

        public static PedidoItemDTO PedidoItemToDtoSet(this PedidoItem item)
        {
            return new PedidoItemDTO
            {
                id = item.id,
                desconto = item.desconto,
                pedidoId = item.pedidoId,
                total = item.total,
                produtoId = item.produtoId,
                valor = item.valor,
                quantidade = item.quantidade
            };
        }

        public static IEnumerable<PedidoItemDTO> PedidoItensToDtoGet(this IEnumerable<PedidoItem> itens)
        {
            return (from item in itens
                    select new PedidoItemDTO
                    {
                        id = item.id,
                        desconto = item.desconto,
                        pedidoId = item.pedidoId,
                        total = item.total,
                        produtoId = item.produtoId,
                        produtoNome = item.produto.nome,
                        valor = item.valor,
                        quantidade = item.quantidade
                    });
        }
    }
}
