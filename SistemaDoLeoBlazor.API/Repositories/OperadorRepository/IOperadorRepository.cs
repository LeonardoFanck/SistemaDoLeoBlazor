using SistemaDoLeoBlazor.API.Entities;

namespace SistemaDoLeoBlazor.API.Repositories.OperadorRepository
{
    public interface IOperadorRepository
    {
        Task<Operador> GetOperadorById(int Id);

        Task<IEnumerable<Operador>> GetOperadores();

        Task<IEnumerable<OperadorTela>> GetTelas(int Id);
    }
}
