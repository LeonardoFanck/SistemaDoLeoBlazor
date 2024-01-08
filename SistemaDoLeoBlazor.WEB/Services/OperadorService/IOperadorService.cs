using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.WEB.Services.OperadorService.OperadorService
{
    public interface IOperadorService
    {
        Task<OperadorDTO> GetOperadorById(int id);
        Task<IEnumerable<OperadorTelaDTO>> GetTelasByOperador(int id);
        Task<OperadorDTO> PostOperador(OperadorDTO operadorDTO);
        Task<OperadorDTO> PostOperadorTelas(OperadorDTO operadorDTO);
        Task<OperadorDTO> PatchOperador(OperadorDTO operadorDTO);
        Task<OperadorTelaDTO> PatchOperadorTelas(OperadorTelaDTO operadorTelaDTO);
        Task<OperadorDTO> DeleteOperador(int id);
        Task<IEnumerable<OperadorDTO>> GetAllOperadores();
    }
}
