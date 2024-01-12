using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.WEB.Services.PedidoService
{
    public interface IPedidoService
    {
        Task<PedidoDTO> GetById(int id);
        Task<IEnumerable<PedidoDTO>> GetAll();
        Task<PedidoDTO> Insert(PedidoDTO pedidoDTO);
        Task<PedidoDTO> Update(PedidoDTO pedidoDTO);
        Task<PedidoDTO> Delete(int id);

        Task<PedidoItemDTO> GetItemById(int id);
        Task<IEnumerable<PedidoItemDTO>> GetAllItens(int id);
        Task<PedidoItemDTO> InsertItem(PedidoItemDTO pedidoItemDTO);
        Task<PedidoItemDTO> UpdateItem(PedidoItemDTO pedidoItemDTO);
        Task<PedidoItemDTO> DeleteItem(int id);
    }
}
