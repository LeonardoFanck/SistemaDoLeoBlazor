using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ProdutoDTO;

namespace SistemaDoLeoBlazor.API.Repositories.ProdutoRepository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> DeleteProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);

            if (produto is not null)
            {
                _context.Produto.Remove(produto);
                await _context.SaveChangesAsync();
            }

            return produto;
        }

        public async Task<IEnumerable<Produto>> GetAllProdutos()
        {
            return await _context.Produto.ToListAsync();
        }

        public async Task<Produto> GetProdutoById(int id)
        {
            return await _context.Produto.FindAsync(id);
        }

        public async Task<Produto> PatchProduto(int id, ProdutoDTO produtoDTO)
        {
            var produto = await _context.Produto.FindAsync(id);

            if (produto is not null)
            {
                produto.nome = produtoDTO.nome;
                produto.categoriaId = produtoDTO.categoriaId;
                produto.custo = produtoDTO.custo;
                produto.estoque = produtoDTO.estoque;
                produto.preco = produtoDTO.preco;
                produto.inativo = produtoDTO.inativo;
                produto.unidade = produtoDTO.unidade;

                await _context.SaveChangesAsync();
            }

            return produto;
        }

        public async Task<Produto> PostProduto(ProdutoDTO produtoDTO)
        {
            var produto = new Produto
            {
                nome = produtoDTO.nome,
                categoriaId = produtoDTO.categoriaId,
                inativo = produtoDTO.inativo,
                custo = produtoDTO.custo,
                estoque = produtoDTO.estoque,
                preco = produtoDTO.preco,
                unidade = produtoDTO.unidade
            };

            var resultado = _context.Produto.AddAsync(produto);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }
    }
}
