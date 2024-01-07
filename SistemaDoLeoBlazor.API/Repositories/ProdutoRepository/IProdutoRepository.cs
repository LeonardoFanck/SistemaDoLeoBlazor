using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ProdutoDTO;

namespace SistemaDoLeoBlazor.API.Repositories.ProdutoRepository
{
    public interface IProdutoRepository
    {
        Task<Produto> GetProdutoById(int id);
        Task<IEnumerable<Produto>> GetAllProdutos();
        Task<Produto> PostProduto(ProdutoDTO produtoDTO);
        Task<Produto> PatchProduto(int id, ProdutoDTO produtoDTO);
        Task<Produto> DeleteProduto(int id);
    }
}
