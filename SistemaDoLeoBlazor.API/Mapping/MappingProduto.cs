using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ProdutoDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingProduto
    {
        public static ProdutoDTO ProdutoToDto(this Produto produto)
        {
            return new ProdutoDTO
            {
                id = produto.id,
                nome = produto.nome,
                categoriaId = produto.categoriaId,
                inativo = produto.inativo,
                custo = produto.custo,
                estoque = produto.estoque,
                preco = produto.preco,
                unidade = produto.unidade
            };
        }

        public static IEnumerable<ProdutoDTO> ProdutosToDto(this IEnumerable<Produto> produtos)
        {
            return (from produto in produtos
                    select new ProdutoDTO
                    {
                        id = produto.id,
                        nome = produto.nome,
                        categoriaId = produto.categoriaId,
                        custo = produto.custo,
                        estoque = produto.estoque,
                        preco = produto.preco,
                        inativo = produto.inativo,
                        unidade = produto.unidade
                    }).ToList();
        }
    }
}
