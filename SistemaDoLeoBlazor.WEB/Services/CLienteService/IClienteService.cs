using SistemaDoLeoBlazor.MODELS.ClienteDTO;

namespace SistemaDoLeoBlazor.WEB.Services.CLienteService
{
    public interface IClienteService
    {
        Task<ClienteDTO> GetById(int id);
        Task<IEnumerable<ClienteDTO>> GetAll();
        Task<ClienteDTO> Insert(ClienteDTO clienteDTO);
        Task<ClienteDTO> Update(ClienteDTO clienteDTO);
        Task<ClienteDTO> Delete(int id);
    }
}
