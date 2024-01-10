using SistemaDoLeoBlazor.MODELS.ProdutoDTO;

namespace SistemaDoLeoBlazor.WEB.Services.ProdutoService
{
    public interface IProdutoService
    {
        Task<ProdutoDTO> GetById(int id);
        Task<IEnumerable<ProdutoDTO>> GetAll();
        Task<ProdutoDTO> Insert(ProdutoDTO produtoDTO);
        Task<ProdutoDTO> Update(ProdutoDTO produtoDTO);
        Task<ProdutoDTO> Delete(int id);
    }
}
