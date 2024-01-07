using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;

namespace SistemaDoLeoBlazor.API.Repositories.FormaPgtoRepository
{
    public interface IFormaPgtoRepository
    {
        Task<FormaPgto> GetFormaPgtoById(int id);
        Task<IEnumerable<FormaPgto>> GetAllFormaPgto();
        Task<FormaPgto> PostFormaPgto(FormaPgtoDTO formaPgtoDTO);
        Task<FormaPgto> PatchFormaPgto(int id, FormaPgtoDTO formaPgtoDTO);
        Task<FormaPgto> DeleteFormaPgto(int id);
    }
}
