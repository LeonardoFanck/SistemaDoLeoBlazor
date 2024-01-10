using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;

namespace SistemaDoLeoBlazor.WEB.Services.FormaPgtoService
{
    public interface IFormaPgtoService
    {
        Task<FormaPgtoDTO> GetById(int id);
        Task<IEnumerable<FormaPgtoDTO>> GetAll();
        Task<FormaPgtoDTO> Insert(FormaPgtoDTO formaPgtoDTO);
        Task<FormaPgtoDTO> Update(FormaPgtoDTO formaPgtoDTO);
        Task<FormaPgtoDTO> Delete(int id);
    }
}
