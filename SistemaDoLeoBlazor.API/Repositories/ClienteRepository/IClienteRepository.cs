using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;

namespace SistemaDoLeoBlazor.API.Repositories.ClienteRepository
{
    public interface IClienteRepository
    {
        Task<Cliente> GetClienteById(int id);
        Task<IEnumerable<Cliente>> GetAllClientes();
        Task<Cliente> PostCliente(ClienteDTO clienteDTO);
        Task<Cliente> PatchCliente(int id, ClienteDTO clienteDTO);
        Task<Cliente> DeleteCliente(int id);
    }
}
