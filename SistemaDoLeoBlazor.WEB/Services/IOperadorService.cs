using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.WEB.Services
{
    public interface IOperadorService
    {
        Task<OperadorDTO> GetOperadorById(int id);
        Task<IEnumerable<OperadorTelaDTO>> GetTelasByOperador(int id);
        Task<OperadorDTO> PostOperador(OperadorDTO operadorDTO);
        Task<OperadorDTO> PostOperadorTelas(OperadorDTO operadorDTO);
        Task<OperadorTelaDTO> PatchOperadorTelas(OperadorTelaDTO operadorTelaDTO);
    }
}
