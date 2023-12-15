using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.API.Repositories.OperadorRepository
{
    public interface IOperadorRepository
    {
        Task<Operador> GetOperadorById(int Id);

        Task<IEnumerable<Operador>> GetOperadores();

        Task<IEnumerable<OperadorTela>> GetTelas(int Id);

        Task<Operador> PostOperador(OperadorDTO operadorDto);
        
        Task<OperadorTela> PostOperadorTela(OperadorTelaDTO operadorTelaDTO);
    }
}
