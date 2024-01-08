using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.WEB.Services.OperadorLocalStorageService
{
    public interface IOperadorLocalService
    {
        Task<OperadorDTO> GetSessaoOperador();
        Task<IEnumerable<OperadorTelaDTO>> GetSessaoTelas();
        Task SetSessao(int id);
        Task LimparSessao();
    }
}
