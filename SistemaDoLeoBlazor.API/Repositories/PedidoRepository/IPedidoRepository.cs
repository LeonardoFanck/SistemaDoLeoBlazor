using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.API.Repositories.PedidoRepository
{
    public interface IPedidoRepository
    {
        Task<Pedido> GetPedidoById(int id);
        Task<IEnumerable<Pedido>> GetAllPedidos();
        Task<Pedido> PostPedido(PedidoDTO pedidoDTO);
        Task<Pedido> PatchPedido(int id, PedidoDTO pedidoDTO);
        Task<Pedido> DeletePedido(int id);

        // ITENS

        Task<IEnumerable<PedidoItem>> GetAllItens(int id);
        Task<PedidoItem> PostItem(PedidoItemDTO itemDto);
        Task<PedidoItem> PatchItem(int id, PedidoItemDTO itemDto);
        Task<PedidoItem> DeleteItem(int id);
    }
}
