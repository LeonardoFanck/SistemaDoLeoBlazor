using SistemaDoLeoBlazor.MODELS.CategoriaDTO;

namespace SistemaDoLeoBlazor.WEB.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<CategoriaDTO> GetById(int id);
        Task<IEnumerable<CategoriaDTO>> GetAll();
        Task<CategoriaDTO> Insert(CategoriaDTO categoriaDTO);
        Task<CategoriaDTO> Update(CategoriaDTO categoriaDTO);
        Task<CategoriaDTO> Delete(int id);
    }
}
