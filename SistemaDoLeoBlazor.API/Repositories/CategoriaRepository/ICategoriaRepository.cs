using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;

namespace SistemaDoLeoBlazor.API.Repositories.CategoriaRepository
{
    public interface ICategoriaRepository
    {
        Task<Categoria> GetCategoriaById(int id);
        Task<IEnumerable<Categoria>> GetAllCategorias();
        Task<Categoria> PostCategoria(CategoriaDTO categoriaDTO);
        Task<Categoria> PatchCategoria(int id, CategoriaDTO categoriaDTO);
        Task<Categoria> DeleteCategoria(int id);
    }
}
